using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Yanz.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            var a = 1;
            return View();
        }
    }
}