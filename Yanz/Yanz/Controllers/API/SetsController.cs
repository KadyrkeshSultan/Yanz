using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yanz.Data;
using Yanz.Models;
using Yanz.Models.Quiz.Public;

namespace Yanz.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Sets")]
    [Authorize]
    public class SetsController : Controller
    {
        ApplicationDbContext db;
        UserManager<ApplicationUser> userManager;

        public SetsController(ApplicationDbContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyPublicSetsAsync()
        {
            var userId = userManager.GetUserId(User);
            var sets = await db.Sets
                .Include(s => s.Questions)
                .Include(s => s.ApplicationUser)
                .Where(s => s.ApplicationUserId == userId)
                .ToListAsync();
            List<SetView> views = new List<SetView>();
            foreach (var s in sets)
                views.Add(new SetView(s));
            return Ok(views);
        }
        
        [HttpGet("messages")]
        public async Task<IActionResult> GetMyModerMsgs()
        {
            var userId = userManager.GetUserId(User);
            var messages = await db.ModerMsgs.Where(m => m.ApplicationUserId == userId).OrderByDescending(m=>m.Create).ToListAsync();
            return Ok(messages);
        }
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = userManager.GetUserId(User);
            var set = await db.Sets.FirstOrDefaultAsync(s => s.Id == id && s.ApplicationUserId == id);
            if (set == null)
                return NotFound(id);
            db.Sets.Remove(set);
            await db.SaveChangesAsync();
            return new StatusCodeResult(204);
        }
    }
}
