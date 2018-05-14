using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yanz.Data;
using Yanz.Models;
using Yanz.Models.Quiz;

namespace Yanz.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Folders")]
    public class FoldersController : Controller
    {
        ApplicationDbContext db;
        UserManager<ApplicationUser> userManager;

        public FoldersController(ApplicationDbContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        [HttpGet]
        [Authorize]
        [Route("root")]
        public IActionResult Root()
        {
            return Ok(GetItems(null));
        }

        //TODO: придется требовать передачи и parentId, Title
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> Patch(string Id, [FromBody]FolderView folderView)
        {
            if (folderView == null)
                return BadRequest();
            var folder = await db.Folder.FirstOrDefaultAsync(f => f.Id == Id);
            if (folder == null)
                return BadRequest();

            //Если перемещяем в root(null)
            if (folderView.ParentId == "root")
                folder.ParentId = null;
            else
            {
                //Если это не root, проверяем есть ли нужная папка
                if (await db.Folder.FirstOrDefaultAsync(f => f.Id == folderView.ParentId) == null)
                    return BadRequest();
            }

            if (folderView.Title?.Length < 1)
                return BadRequest();

            folder.Title = folderView.Title;
            folder.ParentId = folderView.ParentId;
            db.Folder.Update(folder);
            await db.SaveChangesAsync();
            FolderView view = new FolderView(folder, GetBreadcrumbs(folder.ParentId), GetItems(folder.Id));
            return Ok(view);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(string Id)
        {
            var folder = await db.Folder.FirstOrDefaultAsync(f => f.Id == Id);
            if (folder == null)
                return NotFound();

            FolderView view = new FolderView(folder, GetBreadcrumbs(folder.ParentId), GetItems(folder.Id));
            return Ok(view);
        }

        // POST: api/Folders
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]FolderView folder)
        {
            var user = await userManager.GetUserAsync(User);
            if (folder == null || folder.Title.Length < 1)
                return BadRequest();

            var nFolder = new Folder()
            {
                Id = Guid.NewGuid().ToString(),
                Title = folder.Title,
                ApplicationUser = user,
                ParentId = db.Folder.FirstOrDefault(f => f.Id == folder.ParentId)?.Id
            };
            await db.Folder.AddAsync(nFolder);
            await db.SaveChangesAsync();
            FolderView view = new FolderView(nFolder, GetBreadcrumbs(nFolder.ParentId), GetItems(nFolder.Id));
            return Ok(view);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string Id)
        {
            var folder = await db.Folder.FirstOrDefaultAsync(f => f.Id == Id);
            if (folder == null)
                return BadRequest();
            var listFoldersDelete = new List<Folder>();
            var listQstSetsDelete = new List<QuestionSet>();

            GetChildFolders(folder.Id, listFoldersDelete, listQstSetsDelete);

            db.QuestionSets.RemoveRange(listQstSetsDelete);
            db.Folder.RemoveRange(listFoldersDelete);

            db.Folder.Remove(folder);
            await db.SaveChangesAsync();
            return new StatusCodeResult(204);
        }

        /// <summary>
        /// Получем все дочерние элементы, папки, группы вопросов из папки с ID - folderId
        /// </summary>
        /// <param name="folderId">ID папки, из которой нужно получить папки, группы</param>
        /// <param name="foldersDelete">Список папок</param>
        /// <param name="qstSetDelete">Список групп вопросов</param>
        private void GetChildFolders(string folderId, List<Folder> foldersDelete, List<QuestionSet> qstSetDelete)
        {
            var folders = db.Folder.Where(f => f.ParentId == folderId).ToList();

            if (folders == null)
            {
                qstSetDelete.AddRange(db.QuestionSets.Where(q => q.FolderId == folderId).ToList());
                return;
            }
            foldersDelete.AddRange(folders);
            foreach (var folder in folders)
            {
                qstSetDelete.AddRange(db.QuestionSets.Where(q => q.FolderId == folderId).ToList());
                GetChildFolders(folder.Id, foldersDelete, qstSetDelete);
            }
        }

        private List<Item> GetItems(string Id)
        {
            var items = new List<Item>();
            var listFolders = db.Folder.Where(f => f.ParentId == Id).OrderBy(f => f.Title).ToList();
            foreach(var folder in listFolders)
                items.Add(new Item(folder));

            var listQstSets = db.QuestionSets
                .Include(q => q.Questions)
                .Where(q => q.FolderId == Id)
                .OrderBy(q => q.Title).ToList();

            foreach (var qstSet in listQstSets)
                items.Add(new Item(qstSet));

            return items;
        }
        private List<Breadcrumb> GetBreadcrumbs(string parentId)
        {
            if (parentId == null)
                return new List<Breadcrumb>();
            var breadcrumbs = new List<Breadcrumb>();
            Folder folder = db.Folder.FirstOrDefault(f => f.Id == parentId);
            while (folder != null)
            {
                breadcrumbs.Add(new Breadcrumb(folder));
                folder = db.Folder.FirstOrDefault(f => f.Id == folder.ParentId);
            }
            breadcrumbs.Reverse();
            return breadcrumbs;
        }
    }
}