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
    [Route("api/QuestionSets")]
    public class QuestionSetsController : Controller
    {
        ApplicationDbContext db;
        UserManager<ApplicationUser> userManager;

        public QuestionSetsController(ApplicationDbContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        // GET: api/Images
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var userId = userManager.GetUserId(User);
            var sets = db.QuestionSets
                .Include(q => q.ApplicationUser)
                .Include(q => q.Questions)
                .Where(q => q.ApplicationUserId == userId).ToList();
            List<QuestionSetView> views = new List<QuestionSetView>();
            foreach (var set in sets)
                views.Add(new QuestionSetView(set, await GetBreadcrumbsAsync(set.Id)));
            return Ok(views);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(string Id)
        {
            QuestionSet set = await db.QuestionSets.Include(q => q.ApplicationUser).Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == Id);
            if (set == null)
                return NotFound(Id);
            QuestionSetView view = new QuestionSetView(set, await GetBreadcrumbsAsync(Id));
            return Ok(view);
        }

        // POST: api/QuestionSets
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody]QuestionSetView set)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.GetUserAsync(User);
            var folderId = (await db.Folders.FirstOrDefaultAsync(f => f.Id == set.FolderId))?.Id;
            if (set.FolderId != "root" && folderId == null)
                return BadRequest($"Not found folder {set.FolderId}");

            set.FolderId = folderId;

            QuestionSet questionSet = new QuestionSet(user, set.Title, DateTime.Now, set.FolderId);
            await db.QuestionSets.AddAsync(questionSet);
            await db.SaveChangesAsync();
            QuestionSetView view = new QuestionSetView(questionSet, await GetBreadcrumbsAsync(questionSet.Id));
            return Ok(view);
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> Patch(string Id, [FromBody]QuestionSetView set)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            QuestionSet questionSet = await db.QuestionSets
                .Include(q => q.ApplicationUser)
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == Id);

            if (questionSet == null)
                return NotFound(Id);
            Folder folder = await db.Folders.FirstOrDefaultAsync(f => f.Id == set.FolderId);
            if (set.FolderId != "root" && folder == null)
                return BadRequest($"Not found folder {set.FolderId}");
            questionSet.FolderId = folder?.Id;
            questionSet.Title = set.Title;

            db.QuestionSets.Update(questionSet);
            await db.SaveChangesAsync();

            QuestionSetView view = new QuestionSetView(questionSet, await GetBreadcrumbsAsync(questionSet.Id));
            return Ok(view);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string Id)
        {
            var set = await db.QuestionSets.FirstOrDefaultAsync(f => f.Id == Id);
            if (set == null)
                return NotFound(Id);
            db.QuestionSets.Remove(set);
            await db.SaveChangesAsync();
            return new StatusCodeResult(204);
        }

        private async Task<List<Breadcrumb>> GetBreadcrumbsAsync(string setId)
        {
            if (setId == null)
                return new List<Breadcrumb>();

            var userId = userManager.GetUserId(User);
            var breadcrumbs = new List<Breadcrumb>();
            QuestionSet set = await db.QuestionSets.FirstOrDefaultAsync(q => q.Id == setId);
            Folder parentFolder = await db.Folders.FirstOrDefaultAsync(f => f.Id == set.FolderId);
            while (parentFolder != null)
            {
                breadcrumbs.Add(new Breadcrumb(parentFolder));
                parentFolder = await db.Folders.FirstOrDefaultAsync(f => f.Id == parentFolder.ParentId);
            }
            breadcrumbs.Reverse();
            return breadcrumbs;
        }
    }
}