using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization.Json;
using Talker.Controllers.Other;
using Talker.Models;
using Talker.Models.DTO;
using Talker.Models.DTO.Requests;

namespace Talker.Controllers
{
    public class MainController : Controller
    {

        public static string chatViewPath = ($"http://localhost:{BridgeSettings.WebPort}/Chat");


        public static async Task<BrowserWindow> OpenChatDialog(int userId)
        {


            var browserWindow = await HelperMethods.GetFinwodWindowAsync($"Chat?user={userId}&");

            if (browserWindow != null){
                return browserWindow;
            }else
            {
            
            }


            browserWindow = await Electron.WindowManager.CreateWindowAsync(
            new BrowserWindowOptions
            {
                Show = false,
                Frame = false,
                Resizable = true,

                Width = 470,
                Height = 550,

                MinHeight = 300,
                MinWidth = 320,


                WebPreferences = new WebPreferences
                {
                    
                    AllowRunningInsecureContent = true,
                    NodeIntegration = true,
                    //NodeIntegrationInWorker = true,
                    WebSecurity = false,

                },

            }
            , $"{chatViewPath}?user={userId}&");

            //browserWindow.RemoveMenu();
            browserWindow.OnReadyToShow += () => browserWindow.Show();
            
            return browserWindow;
        }



        public async Task<IActionResult> IndexAsync()
        {

            if (HybridSupport.IsElectronActive)
            {


                Electron.IpcMain.On("show-user-conversation-chat", async (args) =>
                {
                    try
                    {
                        var chatWindow = await OpenChatDialog(Convert.ToInt32(args));
                        chatWindow.Focus();
                    }
                    catch (Exception ex)
                    {

                        Electron.Dialog.ShowErrorBox(ex.Message, "");
                    }
                });





                Electron.IpcMain.On("user-profile", async (args) =>
                {

                    BrowserWindow mainWindow = await HelperMethods.GetFinwodWindowAsync("Main");

                    try
                    {
                        await UserMainProfile.UserProfileAsyncRequest();


                        if (mainWindow != null)
                        {

                            Electron.IpcMain.Send(mainWindow, "user-profile-reply", (JsonConvert.SerializeObject(UserMainProfile.UserProfile).ToString()));

                        }
                    }

                    catch (Exception ex)
                    {
                        HelperMethods.DisplayErrorMessage(ex, mainWindow, "auth-login-reply", true);
                    }

                });








                Electron.IpcMain.On("request-messages", async (args) =>
                {



                    string returnChannel = "request-messages-reply";

                    BrowserWindow mainWindow = null;


                    Dictionary<string, Int32> output = null;

                    try
                    {
                        output = JsonConvert.DeserializeObject<Dictionary<string, Int32>>(args.ToString());

                        mainWindow = await HelperMethods.GetFinwodWindowAsync($"Chat?user={output["receiverId"]}&");
                    }
                    catch (Exception ex)
                    {

                        Electron.Dialog.ShowErrorBox(ex.Message, "");
                    }

                    try
                    {
                        Int32 receiverUserId = output["receiverId"];

                        Int32 lastMessageId = output["lastMessageId"];

                        UserRequest ur = new UserRequest { receiverId = receiverUserId, lastMessageId = lastMessageId };

                        List<Message> messages = await UserMainProfile.UserMessagesWithSelectedUserAsyncRequest(ur);


                        Console.WriteLine(JsonConvert.SerializeObject(messages).ToString());

                        Electron.IpcMain.Send(mainWindow, returnChannel, (JsonConvert.SerializeObject(messages).ToString()));

                    }

                    catch (Exception ex)
                    {
                        HelperMethods.DisplayErrorMessage(ex, mainWindow, returnChannel, true);
                    }
                });



                Electron.IpcMain.On("send-message", async (args) =>
                {

                    var output = JsonConvert.DeserializeObject<Dictionary<string, string>>(args.ToString());

                    Int32 receiverUserId = Int32.Parse(output["receiverId"]);

                    string message = output["message"];

                    string returnChannel = "send-message-reply";


                    BrowserWindow mainWindow = null;
                    try
                    {

                        mainWindow = await HelperMethods.GetFinwodWindowAsync($"Chat?user={receiverUserId}");

                        string url = await mainWindow.WebContents.GetUrl();
                    }
                    catch (Exception ex)
                    {

                        Electron.Dialog.ShowErrorBox(ex.Message, "");
                    }


                    try
                    {
                        Message messageRequest = new Message { receiverId = receiverUserId, message = message };

                        Message messageRespond = await UserMainProfile.SendMessageToSelectedUserAsyncRequest(messageRequest);

                        Electron.IpcMain.Send(mainWindow, returnChannel, (JsonConvert.SerializeObject(messageRespond).ToString()));

                    }
                    catch (Exception ex)
                    {
                        HelperMethods.DisplayErrorMessage(ex, mainWindow, returnChannel, true);
                    }


                });


                Electron.IpcMain.On("add-new-friend", async (args) =>
                {

                    string returnChannel = "add-new-friend-reply";

                    BrowserWindow mainWindow = await HelperMethods.GetFinwodWindowAsync("Main");

                    try
                    {
                        UserRequest userRequest = new UserRequest { username = args.ToString() };

                        var messageRespond = await UserMainProfile.AddFriendAsyncRequest(userRequest);

                        Console.WriteLine(JsonConvert.SerializeObject(messageRespond).ToString());

                        await UserMainProfile.UserProfileAsyncRequest();

                        if (mainWindow != null)
                        {
                            Electron.IpcMain.Send(mainWindow, "user-profile-reply", (JsonConvert.SerializeObject(UserMainProfile.UserProfile).ToString()));

                        }

                        Electron.IpcMain.Send(mainWindow, returnChannel, (JsonConvert.SerializeObject(messageRespond).ToString()));

                    }
                    catch (Exception ex)
                    {
                        HelperMethods.DisplayErrorMessage(ex, mainWindow, returnChannel, false);
                    }
                });



                Electron.IpcMain.On("open-external-link", async (args) =>
                {
                    await Electron.Shell.OpenExternalAsync(args.ToString());
                });


                SocketChangesUpdater socketChangesUpdater = null;

                Electron.IpcMain.On("echo-socket-updater-init", async (args) =>
                {
                    BrowserWindow mainWindow = await HelperMethods.GetFinwodWindowAsync("Main");
                    socketChangesUpdater = new SocketChangesUpdater(mainWindow);
                });



                Electron.IpcMain.On("echo-socket-updater", async (args) =>
                {

                    if(socketChangesUpdater != null) socketChangesUpdater.RunEchoSocketChangesUpdaterAsync();
                
                });


                
            }


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}