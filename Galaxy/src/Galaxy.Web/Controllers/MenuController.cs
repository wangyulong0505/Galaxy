﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Galaxy.Web.Controllers
{
    public class MenuController : GalaxyControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}