using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Talker.Controllers.Other;
using Talker.Models;
using Talker.Models.DTO;
using Talker.Models.DTO.Requests;

namespace Talker.Controllers
{
    public class StartController : Controller
    {




        string mainViewPath = ($"http://localhost:{BridgeSettings.WebPort}/Main");

        string registerViewPath = ($"http://localhost:{BridgeSettings.WebPort}/Register");



        public async void openMainWindow(Token userToken)
        {

            //var mainBrowserWindow = Electron.WindowManager.BrowserWindows.First();


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
                        /*NodeIntegration = true,*/

                        AllowRunningInsecureContent = true,
                        WebSecurity = false,
                    },

                }
                , (mainViewPath)) ;
            /*browserWindow.OnMove += UpdateReply;
            browserWindow.OnResize += UpdateReply;
            browserWindow.OnClosed += WindowClosed;

            browserWindow.OnFocus += () => Electron.IpcMain.Send(mainBrowserWindow, "listen-to-window-focus");
            browserWindow.OnBlur += () => Electron.IpcMain.Send(mainBrowserWindow, "listen-to-window-blur");*/


            browserWindow.OnReadyToShow += () => browserWindow.Show();


        }

        public IActionResult Index()
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
                                        /*NodeIntegration = true,*/

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

                    //Console.WriteLine(eventResponse.username);


                    var mainWindow = Electron.WindowManager.BrowserWindows.First();

                    try
                    {
                        await SessionManager.UserLoginAsyncRequest(eventResponse);

                        await UserMainProfile.UserProfileAsyncRequest();

                        SessionManager.RootPageUrl = await mainWindow.WebContents.GetUrl();

                        Console.WriteLine("ROOT URL " + SessionManager.RootPageUrl);

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



        private async void UpdateReply()
        {
            var browserWindow = Electron.WindowManager.BrowserWindows.Last();
            var size = await browserWindow.GetSizeAsync();
            var position = await browserWindow.GetPositionAsync();
            string message = $"Size: {size[0]},{size[1]} Position: {position[0]},{position[1]}";

            var mainWindow = Electron.WindowManager.BrowserWindows.First();
            Electron.IpcMain.Send(mainWindow, "manage-window-reply", message);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}