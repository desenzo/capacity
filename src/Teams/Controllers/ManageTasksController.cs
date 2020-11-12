﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teams.Services;

namespace Teams.Controllers
{
    public class ManageTasksController : Controller
    {
        private readonly IManageTasksService _manageTasksService;
        private readonly IAccessCheckService _accessCheckService;

        public ManageTasksController(IManageTasksService manageTasksService, IAccessCheckService accessCheckService)
        {
            _manageTasksService = manageTasksService;
            _accessCheckService = accessCheckService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTaskByIdAsync(int teamId, int taskId)
        {
            if (!await _accessCheckService.OwnerOrMemberAsync(teamId))
            {
                return View("ErrorGetTaskById");
            }

            var task = await _manageTasksService.GetTaskByIdAsync(taskId);
            return View(task);
        }
    }
}