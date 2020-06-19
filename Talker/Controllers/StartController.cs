using System;
using System.Diagnostics;
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

    /// <summary>
    /// 
    /// Converts login data given by user and sends it to authorization model to verify credicals on server side 
    /// Handles user authorization
    /// This class inherits form AspNetCore Controller class:
    /// A base class for MVC controller with view support.
    /// 
    /// Detailed Login View description:
    /// Graphical interface used for logging into the chat platform - login, password text areas and login button
    /// 
    /// </summary>
    public class StartController : Controller
    {

        private readonly string mainViewPath = ($"http://localhost:{BridgeSettings.WebPort}/Main");
        private readonly string registerViewPath = ($"http://localhost:{BridgeSettings.WebPort}/Register");

        /// <summary>
        /// 
        /// Method that opens Main Window of the program after successful authorization.
        /// 
        /// </summary>
        public async void openMainWindow(Token userToken)
        {

            var browserWindow = await Electron.WindowManager.CreateWindowAsync(
                new BrowserWindowOptions
                {
                    Show = false,
                    Frame = false,
                    Resizable = true,

                    Width = 345,
                    Height = 640,

                    MinHeight = 350,
                    MinWidth = 320,

                    MaxWidth = 600,
                    Maximizable = false,

                    WebPreferences = new WebPreferences
                    {
                        AllowRunningInsecureContent = true,
                        WebSecurity = false,
                        DevTools = false
                    },

                }
                , (mainViewPath)) ;


            browserWindow.OnReadyToShow += () => browserWindow.Show();


        }


        /// <summary>
        /// 
        ///  Asynchronous method that enrolls endpoints used in the login view.
        /// 
        /// </summary>
        public async Task<IActionResult> IndexAsync()
        {

            if (HybridSupport.IsElectronActive)
            {

                Electron.IpcMain.On("button-cl", async (args) => {
                    Electron.App.Quit();
                });


                Electron.IpcMain.On("open-register-dialog", async (args) =>
                {

                    var bw = await HelperMethods.GetFinwodWindowAsync($"Register");

                    if (bw == null)
                    {
                        var browserWindow = await Electron.WindowManager.CreateWindowAsync(
                                new BrowserWindowOptions
                                {
                                    Show = false,
                                    Frame = false,
                                    Resizable = true,

                                    Width = 400,
                                    Height = 650,

                                    MinHeight = 350,
                                    MinWidth = 320,

                                    MaxWidth = 600,

                                    Maximizable = false,

                                    WebPreferences = new WebPreferences
                                    {
                                        AllowRunningInsecureContent = true,
                                        WebSecurity = false,
                                    },

                                }
                                , registerViewPath); ;

                        browserWindow.RemoveMenu();
                        browserWindow.OnReadyToShow += () => browserWindow.Show();
                    }
                    else
                    {
                        bw.Focus();
                    }

                });



                Electron.IpcMain.On("auth-login", async (data) =>
                {

                    var eventResponse = JsonConvert.DeserializeObject<UserRequest>(data.ToString());
                    var mainWindow = Electron.WindowManager.BrowserWindows.First();

                    try
                    {
                        await SessionManager.UserLoginAsyncRequest(eventResponse);
                        await UserMainProfile.UserProfileAsyncRequest();
                        
                        SessionManager.RootPageUrl = await mainWindow.WebContents.GetUrl();
                        Electron.IpcMain.Send(mainWindow, "auth-login-reply", SessionManager.UserToken);

                        if (SessionManager.UserToken.token != null)
                        {
                            openMainWindow(SessionManager.UserToken);
                            mainWindow.Close();
                        }

                    }
                    catch (Exception ex)
                    {

                        HelperMethods.DisplayErrorMessage(ex, mainWindow, "auth-login-reply", false);
                    }
                });




                Electron.IpcMain.On("close-window", async (args) =>
                {
                try
                {
                    BrowserWindow window = await HelperMethods.GetFinwodWindowAsync(Convert.ToString(args));

                    if (Convert.ToString(args).Contains("Chat")) {

                            string id = Convert.ToString(args).Replace("Chat?user=", "");
                            id = id.Replace("&", "");
                            Int32 user = Int32.Parse(id);

                            UserMainProfile.ClearMessagesWithSelectedUserAsyncRequest(new UserRequest { receiverId = user });

                        }

                    window.Destroy();
                    
                }
                catch (Exception ex)
                {
                   Electron.Dialog.ShowErrorBox(ex.Message, ex.InnerException.Message);
                 }

                });



                Electron.IpcMain.On("minimalize-window", async (args) =>
                {
                    BrowserWindow window = await HelperMethods.GetFinwodWindowAsync(Convert.ToString(args));
                    window.Minimize();

                });



                Electron.IpcMain.On("maximize-window", async (args) =>
                {
                    BrowserWindow window = await HelperMethods.GetFinwodWindowAsync(Convert.ToString(args));

                    if (await window.IsMaximizedAsync())
                    {
                        window.Restore();
                    }
                    else{

                        window.Maximize();
                    }

                });


            }


            return View();
        }

        /// <summary>
        /// 
        ///  The Developer Exception Page is used to get detailed stack traces for runtime view errors.
        /// 
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}