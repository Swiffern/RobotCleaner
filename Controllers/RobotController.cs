﻿using Microsoft.AspNetCore.Mvc;
using RobotCleaner.Containers;
using RobotCleaner.Data.Models;

namespace RobotCleaner.Controllers
{
    [ApiController]
    [Route("/tibber-developer-test/enter-path")]
    public class RobotController : ControllerBase
    {
        private ICleaner _cleaner;

        public RobotController(ICleaner cleaner)
        {
            _cleaner = cleaner;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "What can I clean for you? :)";
        }

        [HttpPost]
        public ActionResult<CleaningResult> Post(Instructions instructions)
        {
            return _cleaner.Clean(instructions);
        }
    }
}
