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
using Yanz.Models.Quiz.Public;

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
                .AsNoTracking()
                .Include(q => q.ApplicationUser)
                .Include(q => q.Questions)
                .Where(q => q.ApplicationUserId == userId)
                .OrderBy(o => o.Title);
            List<QuestionSetView> views = new List<QuestionSetView>();
            foreach (var set in sets)
                views.Add(new QuestionSetView(set, await GetBreadcrumbsAsync(set.Id)));
            return Ok(views);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(string Id)
        {
            QuestionSet set = await db.QuestionSets
                .AsNoTracking()
                .Include(q => q.ApplicationUser)
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == Id);
            if (set == null)
                return NotFound(Id);
            //set.Questions = set.Questions.OrderBy(q => q.Order).ToList();
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

            QuestionSet questionSet = new QuestionSet(user, set.Title, DateTime.Now, set.FolderId, set.Image, set.Desc);
            await db.QuestionSets.AddAsync(questionSet);
            await db.SaveChangesAsync();
            QuestionSetView view = new QuestionSetView(questionSet, await GetBreadcrumbsAsync(questionSet.Id));
            return Ok(view);
        }

        [HttpPost("{id}/associate")]
        [Authorize]
        public async Task<IActionResult> Associate(string id, string[] qsts)
        {
            var set = await db.QuestionSets.FirstOrDefaultAsync(q => q.Id == id);
            if (set == null)
                return NotFound(id);
            List<Question> questions = new List<Question>();
            foreach (var quest in qsts)
            {
                var q = await db.Questions.FirstOrDefaultAsync(c => c.Id == quest);
                if (q == null)
                    return BadRequest(quest);
                q.QuestionSetId = set.Id;
                questions.Add(q);
            }
            
            db.Questions.UpdateRange(questions);
            await db.SaveChangesAsync();
            return Ok(qsts);
        }
        
        [HttpPost("{id}/reorder")]
        [Authorize]
        public async Task<IActionResult> Reorder(string id, string[] qsts)
        {
            var set = await db.QuestionSets.FirstOrDefaultAsync(q => q.Id == id);
            if (set == null)
                return NotFound(id);

            List<Question> questions = new List<Question>();
            int order = 0;
            foreach (var quest in qsts)
            {
                var q = await db.Questions.FirstOrDefaultAsync(c => c.Id == quest);
                if (q == null)
                    return BadRequest(quest);
                q.Order = order;
                questions.Add(q);
                order++;
            }
            db.Questions.UpdateRange(questions);
            await db.SaveChangesAsync();
            return Ok(qsts);
        }

        [HttpPost("{id}/publish")]
        [Authorize]
        public async Task<IActionResult> Publish(string id)
        {
            var set = await db.QuestionSets.FirstOrDefaultAsync(q => q.Id == id);
            if (set == null)
                return NotFound(id);
            string userId = userManager.GetUserId(User);
            var msg = await db.ModerMsgs.FirstOrDefaultAsync(m => m.QuestionSetId == id);
            if(msg == null)
            {
                ModerMsg m = new ModerMsg()
                {
                    Id = Guid.NewGuid().ToString(),
                    Create = DateTime.Now,
                    Status = Status.Moderation,
                    ApplicationUserId = userId,
                    QuestionSetId = id
                };
                db.ModerMsgs.Add(m);
                await db.SaveChangesAsync();
                return Ok(m);
            }
            if (msg?.Status == Status.Moderation)
                return BadRequest($"{id} on moderation");
            msg.Status = Status.Moderation;
            db.ModerMsgs.Update(msg);
            await db.SaveChangesAsync();
            return Ok(msg);
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

            //Опасная зона
            set.FolderId = folder?.Id;
            questionSet.Update(set);

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