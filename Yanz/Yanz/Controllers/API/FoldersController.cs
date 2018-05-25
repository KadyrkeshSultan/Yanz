using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Root()
        {
            return Ok(await GetItemsAsync(null));
        }


        //TODO: поискать решение с partial Patch
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> Patch(string Id, [FromBody]FolderView folderView)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = userManager.GetUserId(User);

            Folder folder = await db.GetFolderAsync(userId, Id);

            if (folder == null)
                return NotFound(Id);

            string moveResult = await db.MoveAsync(userId, Id, folderView.ParentId);
            if (moveResult != null)
                return BadRequest(moveResult);

            string renameResult = await db.RenameAsync(userId, Id, folderView.Title);
            if (renameResult != null)
                return BadRequest(renameResult);

            await db.SaveAsync();
            FolderView view = new FolderView(folder, await GetBreadcrumbsAsync(folder.Id), await GetItemsAsync(folder.Id));
            return Ok(view);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(string Id)
        {
            var userId = userManager.GetUserId(User);
            Folder folder = await db.GetFolderAsync(userId, Id);
            if (folder == null)
                return NotFound(Id);

            FolderView view = new FolderView(folder,await GetBreadcrumbsAsync(folder.Id), await GetItemsAsync(folder.Id));
            return Ok(view);
        }

        // POST: api/Folders
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]FolderView folder)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string userId = userManager.GetUserId(User);
            string parentId = (await db.GetFolderAsync(userId, folder.ParentId))?.Id;
            if (folder.ParentId != "root" && parentId == null)
                return BadRequest($"Not found folder {folder.ParentId}");

            var nFolder = new Folder(folder.Title, userId, parentId);
            await db.AddAsync(nFolder);
            await db.SaveAsync();

            FolderView view = new FolderView(nFolder, await GetBreadcrumbsAsync(nFolder.Id), await GetItemsAsync(nFolder.Id));
            return Ok(view);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string Id)
        {
            string userId = userManager.GetUserId(User);
            bool onDelete = await db.RemoveAsync(userId, Id);
            if (!onDelete)
                return BadRequest(Id);
            await db.SaveAsync();
            return new StatusCodeResult(204);
        }

        /// <summary>
        /// Получаем все элементы(набор вопросов, подпапки) в папке
        /// </summary>
        /// <param name="folderId">ID папки</param>
        /// <returns></returns>
        private async Task<List<Item>> GetItemsAsync(string folderId)
        {
            string userId = userManager.GetUserId(User);
            var items = new List<Item>();
            var listFolders = (await db.GetFoldersAsync(userId))
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
        private async Task<List<Breadcrumb>> GetBreadcrumbsAsync(string folderId)
        {
            if (folderId == null)
                return new List<Breadcrumb>();

            string userId = userManager.GetUserId(User);
            var breadcrumbs = new List<Breadcrumb>();
            Folder folder = await db.GetFolderAsync(userId, folderId);
            Folder parentFolder = await db.GetFolderAsync(userId, folder.ParentId);
            while (parentFolder != null)
            {
                breadcrumbs.Add(new Breadcrumb(parentFolder));
                parentFolder = await db.GetFolderAsync(userId, parentFolder.ParentId);
            }
            breadcrumbs.Reverse();
            return breadcrumbs;
        }
    }
}