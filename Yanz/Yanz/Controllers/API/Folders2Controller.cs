using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Yanz.Data;
using Yanz.Models;

namespace Yanz.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Folders2")]
    public class Folders2Controller : Controller
    {
        ApplicationDbContext db;
        UserManager<ApplicationUser> userManager;

        public Folders2Controller(ApplicationDbContext _db, UserManager<ApplicationUser> _userManager)
        {
            db = _db;
            userManager = _userManager;
        }
        // GET: api/Folders2
        [HttpGet]
        [Authorize(Roles ="teacher")]
        public async Task<IEnumerable<string>> Get()
        {
            var user = await userManager.GetUserAsync(User);
            return new string[] { "value1", "value2" };
        }

        // GET: api/Folders2/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Folders2
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Folders2/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
