using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using AirplanePlanner.Models;

namespace AirplanePlanner.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet("/")]
        public ActionResult Index()
        {
            return View();
        }

        
    }
}
