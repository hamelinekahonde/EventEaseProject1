﻿using Microsoft.AspNetCore.Mvc;

namespace EventEaseProject.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
