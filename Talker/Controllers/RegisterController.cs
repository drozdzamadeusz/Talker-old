using System;
using System.Threading.Tasks;
using ElectronNET.API;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using Talker.Controllers.Other;
using Talker.Models;
using Talker.Models.DTO.Requests;

namespace Talker.Controllers
{

    /// <summary>
    /// 
    /// Converts registration data given by user and sends it to authorization model to verify credicals on server side 
    /// Handles a new user registration
    /// This class inherits form AspNetCore Controller class:
    /// A base class for MVC controller with view support.
    /// 
    /// Detailed Login View description:
    /// Graphical interface used for logging into the chat platform - login, password text areas and login button
    /// 
    /// </summary>
    public class RegisterController : Controller
    {

        /// <summary>
        /// 
        ///  Asynchronous method that enrolls endpoints used in the login view.
        /// 
        /// </summary>
        public async Task<IActionResult> IndexAsync()
        {
            if (HybridSupport.IsElectronActive)
            {

                Electron.IpcMain.On("auth-register", async (data) =>
                {
                    var channel = "auth-register-reply";
                    var eventResponse = JsonConvert.DeserializeObject<UserRequest>(data.ToString());
                    var mainWindow = await HelperMethods.GetFinwodWindowAsync("Register");

                    try
                    {
                        var response = await SessionManager.UserRegisterAsyncRequest(eventResponse);
                        Electron.IpcMain.Send(mainWindow, channel, JsonConvert.SerializeObject(response).ToString());

                    }
                    catch (Exception ex)
                    {
                        HelperMethods.DisplayErrorMessage(ex, mainWindow, channel, false);
                    }
                });
            }

            return View();
        }
    }
}