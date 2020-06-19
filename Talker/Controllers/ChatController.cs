using System;
using System.Threading.Tasks;
using ElectronNET.API;
using Microsoft.AspNetCore.Mvc;
using Talker.Models;
using Talker.Models.DTO;

namespace Talker.Controllers
{
    /// <summary>
    /// 
    /// Handles one instance of Chat Window with and passes data through view and program model.
    /// This class inherits form Asp Net Core Controller class:
    /// A base class for MVC controller with view support.
    ///
    /// 
    /// Chat View is response for text chat with one selected user from contacts list - we can send and receive messages online.
    /// 
    /// </summary>
    public class ChatController : Controller
    {

        /// <summary>
        /// 
        ///  Method that initialize a new instance of chat window
        /// 
        /// </summary>
        public IActionResult Index()
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
            return View();
        }


        public IActionResult Details()
        {
            return View();
        }
    }
}