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


        //TODO: поискать решение с partial Patch
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> Patch(string Id, [FromBody]FolderView folderView)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var folders = GetFolders();
            var folder = folders.FirstOrDefault(f => f.Id == Id);

            if (folder == null)
                return NotFound(Id);
            var parentId = folders.FirstOrDefault(f => f.Id == folderView.ParentId)?.Id;

            if (folderView.ParentId != "root" && parentId == null)
                return BadRequest($"Not exist folderId = {folderView.ParentId}");

            if (IsSubFolder(Id, folderView.ParentId))
                return BadRequest($"Folder {folderView.ParentId} is sub folder {Id}");

            folder.Title = folderView.Title;
            folder.ParentId = parentId;
            db.Folders.Update(folder);
            await db.SaveChangesAsync();
            FolderView view = new FolderView(folder, GetBreadcrumbs(folder.ParentId), GetItems(folder.Id));
            return Ok(view);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get(string Id)
        {
            var folders = GetFolders();
            var folder = folders.FirstOrDefault(f => f.Id == Id);
            if (folder == null)
                return NotFound(Id);

            FolderView view = new FolderView(folder, GetBreadcrumbs(folder.ParentId), GetItems(folder.Id));
            return Ok(view);
        }

        // POST: api/Folders
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]FolderView folder)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.GetUserAsync(User);

            var nFolder = new Folder()
            {
                Id = Guid.NewGuid().ToString(),
                Title = folder.Title,
                ApplicationUser = user,
                ParentId = GetFolders().FirstOrDefault(f => f.Id == folder.ParentId)?.Id
            };
            await db.Folders.AddAsync(nFolder);
            await db.SaveChangesAsync();
            FolderView view = new FolderView(nFolder, GetBreadcrumbs(nFolder.ParentId), GetItems(nFolder.Id));
            return Ok(view);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string Id)
        {
            var folders = GetFolders();
            var folder = folders.FirstOrDefault(f => f.Id == Id);
            if (folder == null)
                return BadRequest(Id);
            var listFoldersDelete = new List<Folder>();
            var listQstSetsDelete = new List<QuestionSet>();

            //Рекурсивная функция, обход каталога
            GetChildItems(folder.Id, listFoldersDelete, listQstSetsDelete);

            db.QuestionSets.RemoveRange(listQstSetsDelete);
            db.Folders.RemoveRange(listFoldersDelete);

            db.Folders.Remove(folder);
            await db.SaveChangesAsync();
            return new StatusCodeResult(204);
        }

        /// <summary>
        /// Получем все дочерние элементы, папки, группы вопросов из папки с ID - folderId
        /// </summary>
        /// <param name="folderId">ID папки, из которой нужно получить папки, группы</param>
        /// <param name="foldersDelete">Список папок</param>
        /// <param name="qstSetDelete">Список групп вопросов</param>
        private void GetChildItems(string folderId, List<Folder> foldersDelete, List<QuestionSet> qstSetDelete)
        {
            var folders = GetFolders().Where(f => f.ParentId == folderId).ToList();

            if (folders == null)
            {
                qstSetDelete.AddRange(GetQuestionSets().Where(q => q.FolderId == folderId));
                return;
            }
            foldersDelete.AddRange(folders);
            foreach (var folder in folders)
            {
                qstSetDelete.AddRange(GetQuestionSets().Where(q => q.FolderId == folderId));
                GetChildItems(folder.Id, foldersDelete, qstSetDelete);
            }
        }

        /// <summary>
        /// Получаем все элементы(набор вопросов, подпапки) в папке
        /// </summary>
        /// <param name="folderId">ID папки</param>
        /// <returns></returns>
        private List<Item> GetItems(string folderId)
        {
            var items = new List<Item>();
            var listFolders = GetFolders()
                .Where(f => f.ParentId == folderId)
                .OrderBy(f => f.Title)
                .ToList();
            foreach (var folder in listFolders)
                items.Add(new Item(folder));

            var listQstSets = GetQuestionSets().Where(q => q.FolderId == folderId)
                .OrderBy(q => q.Title)
                .ToList();

            foreach (var qstSet in listQstSets)
                items.Add(new Item(qstSet));

            return items;
        }

        /// <summary>
        /// Получаем путь вложенности папки до корня(null)
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<Breadcrumb> GetBreadcrumbs(string parentFolderId)
        {
            if (parentFolderId == null)
                return new List<Breadcrumb>();
            var breadcrumbs = new List<Breadcrumb>();
            Folder folder = GetFolders().FirstOrDefault(f => f.Id == parentFolderId);
            while (folder != null)
            {
                breadcrumbs.Add(new Breadcrumb(folder));
                folder = GetFolders().FirstOrDefault(f => f.Id == folder.ParentId);
            }
            breadcrumbs.Reverse();
            return breadcrumbs;
        }

        /// <summary>
        /// Является ли папка в которую надо переместить - подпапкой
        /// </summary>
        /// <param name="folderId">Id папки которую нужно переместить</param>
        /// <param name="moveFolderId">Id папки в которую нужно переместить</param>
        /// <returns></returns>
        private bool IsSubFolder(string folderId, string moveFolderId)
        {
            var subFolders = new List<Folder>();
            GetSubFolders(folderId, subFolders);
            foreach (var folder in subFolders)
                if (folder.Id == moveFolderId)
                    return true;
            return false;
        }

        private void GetSubFolders(string folderId, List<Folder> subFolders)
        {
            var folders = GetFolders().Where(f => f.ParentId == folderId).ToList();
            subFolders.AddRange(folders);
            foreach (var folder in folders)
                GetSubFolders(folder.Id, subFolders);
        }

        /// <summary>
        /// Получить папки текущего пользователя
        /// </summary>
        /// <returns></returns>
        private List<Folder> GetFolders()
        {
            var userId = userManager.GetUserId(User);
            return db.Folders.Where(f => f.ApplicationUserId == userId).ToList();
        }

        /// <summary>
        /// Получить набор вопросов текущего пользователя
        /// </summary>
        /// <returns>QuestionSets вместе questions</returns>
        private List<QuestionSet> GetQuestionSets()
        {
            var userId = userManager.GetUserId(User);
            return db.QuestionSets
                .Include(q => q.Questions)
                .Where(q => q.ApplicationUserId == userId)
                .ToList();
        }
    }
}