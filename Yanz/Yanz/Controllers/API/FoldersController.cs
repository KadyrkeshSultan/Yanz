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
    [Authorize]
    public class FoldersController : Controller
    {
        FolderContext db;
        UserManager<ApplicationUser> userManager;

        public FoldersController(UserManager<ApplicationUser> _userManager, FolderContext _db)
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

            var userId = userManager.GetUserId(User);

            var folder = db.GetFolder(userId, Id);

            if (folder == null)
                return NotFound(Id);

            //TODO: не обновлять пока, пока оба метода не сработают
            var moveResult = await db.MoveAsync(userId, Id, folderView.ParentId);
            if (moveResult != null)
                return BadRequest(moveResult);

            var renameResult = await db.Rename(userId, Id, folderView.Title);

            if (renameResult != null)
                return BadRequest(renameResult);
            
            FolderView view = new FolderView(folder, GetBreadcrumbs(folder.Id), GetItems(folder.Id));
            return Ok(view);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get(string Id)
        {
            var userId = userManager.GetUserId(User);
            var folder = db.GetFolder(userId, Id);
            if (folder == null)
                return NotFound(Id);

            FolderView view = new FolderView(folder, GetBreadcrumbs(folder.Id), GetItems(folder.Id));
            return Ok(view);
        }

        // POST: api/Folders
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]FolderView folder)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = userManager.GetUserId(User);
            var parentId = db.GetFolder(userId, folder.ParentId)?.Id;

            var nFolder = new Folder(folder.Title, userId, parentId);
            await db.AddAsync(nFolder);
            
            FolderView view = new FolderView(nFolder, GetBreadcrumbs(nFolder.Id), GetItems(nFolder.Id));
            return Ok(view);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string Id)
        {
            var userId = userManager.GetUserId(User);
            var onDelete = await db.RemoveAsync(userId, Id);
            if (!onDelete)
                return BadRequest(Id);
            
            return new StatusCodeResult(204);
        }

        /// <summary>
        /// Получаем все элементы(набор вопросов, подпапки) в папке
        /// </summary>
        /// <param name="folderId">ID папки</param>
        /// <returns></returns>
        private List<Item> GetItems(string folderId)
        {
            var userId = userManager.GetUserId(User);
            var items = new List<Item>();
            var listFolders = db.GetFolders(userId)
                .Where(f => f.ParentId == folderId)
                .OrderBy(f => f.Title)
                .ToList();

            foreach (var folder in listFolders)
                items.Add(new Item(folder));

            var listQstSets = db.GetQuestionSets(folderId).OrderBy(q => q.Title).ToList();

            foreach (var qstSet in listQstSets)
                items.Add(new Item(qstSet));

            return items;
        }

        /// <summary>
        /// Получаем путь вложенности папки до корня(null)
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        private List<Breadcrumb> GetBreadcrumbs(string folderId)
        {
            if (folderId == null)
                return new List<Breadcrumb>();

            var userId = userManager.GetUserId(User);
            var breadcrumbs = new List<Breadcrumb>();
            Folder folder = db.GetFolder(userId, folderId);
            Folder parentFolder = db.GetFolder(userId, folder.ParentId);
            while (parentFolder != null)
            {
                breadcrumbs.Add(new Breadcrumb(parentFolder));
                parentFolder = db.GetFolder(userId, parentFolder.ParentId);
            }
            breadcrumbs.Reverse();
            return breadcrumbs;
        }
    }
}