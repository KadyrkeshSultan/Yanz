using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yanz.Data;
using Yanz.Models.Quiz;
using Yanz.Models.Quiz.Public;

namespace Yanz.Controllers.Admin
{
    [Produces("application/json")]
    [Route("api/Moder")]
    [Authorize(Roles ="admin")]
    public class ModerController : Controller
    {
        ApplicationDbContext db;

        public ModerController(ApplicationDbContext _db)
        {
            db = _db;
        }
        // GET: api/Moder
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var msgs = await db.ModerMsgs.Where(m => m.Status == Status.Moderation).OrderByDescending(m => m.Create).ToListAsync();
            return Ok(msgs);
        }
        
        // POST: api/Moder
        [Authorize(Roles ="admin")]
        [HttpPost("{id}/public")]
        public async Task<IActionResult> Public(string id)
        {
            var msg = await db.ModerMsgs.FirstOrDefaultAsync(m => m.Id == id);
            if (msg == null)
                return NotFound(id);
            var qstSet = await db.QuestionSets.FirstOrDefaultAsync(q => q.Id == msg.QuestionSetId);
            if (qstSet == null)
                return NotFound($"Not found question set {msg.QuestionSetId}");

            Set set = new Set(qstSet);
            List<Question> questions = new List<Question>();
            var setQsts = await db.Questions.Include(q => q.Choices).Where(q => q.QuestionSetId == qstSet.Id).ToListAsync();
            foreach(var q in setQsts)
            {
                Question qst = q.GetForPublicSetCopy(set.Id);
                var choices = new List<Choice>();
                foreach(var c in q.Choices)
                    choices.Add(c.GetCopy(qst.Id));
                qst.Choices = choices;
                questions.Add(qst);
            }

            set.Questions = questions;
            db.Questions.AddRange(questions);
            msg.Status = Status.Public;
            db.Sets.Add(set);
            db.ModerMsgs.Update(msg);
            await db.SaveChangesAsync();
            return Ok();
        }

        // POST: api/Moder
        [HttpPost("{id}/disapproved")]
        public async Task<IActionResult> Disapproved(string id, string text)
        {
            var msg = await db.ModerMsgs.FirstOrDefaultAsync(m => m.Id == id);
            if (msg == null)
                return NotFound(id);
            msg.Status = Status.Disapproved;
            msg.Text = text;
            db.ModerMsgs.Update(msg);
            await db.SaveChangesAsync();
            return Ok(msg);
        }
    }
}
