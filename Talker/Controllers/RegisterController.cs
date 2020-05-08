using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Talker.Controllers.Other;
using Talker.Models;
using Talker.Models.DTO.Requests;

namespace Talker.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
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