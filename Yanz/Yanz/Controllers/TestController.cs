﻿using System;
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
            return View();
        }
        
        public IActionResult Set()
        {
            return View();
        }

        public IActionResult Qst()
        {
            return View();
        }
    }
}