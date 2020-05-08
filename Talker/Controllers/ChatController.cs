using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Talker.Controllers.Other;
using Talker.Models;
using Talker.Models.DTO;
using Talker.Models.DTO.Requests;

namespace Talker.Controllers
{
    public class ChatController : Controller
    {
        public async Task<IActionResult> IndexAsync()
        {

            var receiverUserId = Request.Query["user"];

            User selectedUser = null;
            foreach (User u in UserMainProfile.UserProfile.contacts){
                if (u.id == Int32.Parse(receiverUserId))
                {
                    selectedUser = u;
                    break;
                }
            }

            ViewData["SelectedUser"] = selectedUser;


            if (HybridSupport.IsElectronActive) { }

            return View();

        }


        public IActionResult Details()
        {
            return View();
        }
    }
}