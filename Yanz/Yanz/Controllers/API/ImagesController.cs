using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Yanz.Controllers.API
{
    [Produces("application/json")]
    [Route("api/Images")]
    public class ImagesController : Controller
    {
        // GET: api/Images/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Images
        [HttpPost("{url}")]
        public void Post(string url)
        {
            
        }
    }
}
