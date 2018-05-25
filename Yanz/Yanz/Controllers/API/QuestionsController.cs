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
    [Route("api/Questions")]
    [Authorize]
    public class QuestionsController : Controller
    {
        ApplicationDbContext db;
        UserManager<ApplicationUser> userManager;
        readonly string[] kinds = { "choice", "multiple", "text", "sorter" };

        public QuestionsController(ApplicationDbContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var qst = await db.Questions.Include(q => q.Choices).FirstOrDefaultAsync(q => q.Id == id);
            if (qst == null)
                return NotFound();
            QuestionView view = new QuestionView(qst);
            return Ok(view);
        }
        
        // POST: api/Questions
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]QuestionView question)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string checkKind = CheckKind(question);
            if (!string.IsNullOrEmpty(checkKind))
                return BadRequest(checkKind);

            if (await db.QuestionSets.FirstOrDefaultAsync(q => q.Id == question.QuestionSetId) == null)
                return NotFound(question.QuestionSetId);

            List<Choice> choices = new List<Choice>();
            if (question.Kind != "text")
                foreach (var c in question.Choices.OrderBy(q => q.Order))
                    choices.Add(new Choice(c, question.QuestionSetId));

            Question qst = new Question(question, choices);
            db.Questions.Add(qst);
            await db.SaveChangesAsync();
            QuestionView view = new QuestionView(qst);
            //TODO: сделать Pacth, копию в sessionChoice
            return Ok(view);
        }

        // POST: api/Questions
        [HttpPost("{id}")]
        public async Task<IActionResult> Post(string id, [FromBody]QuestionView question)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var quest = await db.Questions.Include(q => q.Choices).FirstOrDefaultAsync(q => q.Id == id);
            if (quest == null)
                return NotFound();

            string checkKind = CheckKind(question);
            if (!string.IsNullOrEmpty(checkKind))
                return BadRequest(checkKind);

            if (await db.QuestionSets.FirstOrDefaultAsync(q => q.Id == question.QuestionSetId) == null)
                return NotFound(question.QuestionSetId);

            db.Choices.RemoveRange(quest.Choices);

            List<Choice> choices = new List<Choice>();
            if (question.Kind != "text")
                foreach (var c in question.Choices.OrderBy(q => q.Order))
                    choices.Add(new Choice(c, question.QuestionSetId));

            Question nQuestion = new Question(question, choices);

            UpdateQst(quest, nQuestion);

            db.Questions.Update(quest);
            await db.SaveChangesAsync();
            
            QuestionView view = new QuestionView(quest);
            return Ok(view);
        }
        
        private string CheckKind(QuestionView question)
        {
            if (kinds.FirstOrDefault(k => k == question.Kind) == null)
                return $"Kind {question.Kind} does not exist";

            if (question.Choices == null || question.Choices?.Count < 2 && question.Kind != "text")
                return "At least two choices, if this is not text";

            if (question.Kind == "choice" && question.Choices.Count(c => c.IsCorrect == true) != 1)
                return "For the Kind choice, there must be only one isCorrect";

            if (question.Kind == "multiple" && question.Choices.Count(c => c.IsCorrect == true) < 1)
                return "For the Kind multiple, at least one isCorrect";
            return string.Empty;
        }

        /// <summary>
        /// Обновляем все свойства кроме Id
        /// </summary>
        /// <param name="old">То что нужно обновить</param>
        /// <param name="newQ">На то что нужно обновить</param>
        private void UpdateQst(Question old, Question newQ)
        {
            old.Choices = newQ.Choices;
            old.Content = newQ.Content;
            old.Created = newQ.Created;
            old.Explanation = newQ.Explanation;
            old.Image = newQ.Image;
            old.IsPoll = newQ.IsPoll;
            old.IsTrueCorrect = newQ.IsTrueCorrect;
            old.Kind = newQ.Kind;
            old.Modified = newQ.Modified;
            old.QuestionSetId = newQ.QuestionSetId;
            old.Title = newQ.Title;
            old.Weight = newQ.Weight;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var qst = await db.Questions.FirstOrDefaultAsync(q => q.Id == id);
            if (qst == null)
                return NotFound();

            db.Questions.Remove(qst);
            await db.SaveChangesAsync();
            return new StatusCodeResult(204);
        }
    }
}
