using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Exchanges information form Main Window and all active Chat Windows and passes it through view and program model.
    /// Handles the most of program (API) endpoints.
    /// This class inherits form Asp Net Core Controller class:
    /// A base class for MVC controller with view support.
    /// 
    /// Detailed Main View and Chat View description:
    /// GUI with the list of users saved as friendscontacts - nicknames, statuses, etc.
    /// Menu allows to edit contacts list (add or remove). On the top basic information about logged user.
    /// By clicking on the one of contact, user can initiate chat with person selected.
    /// Main window of program, very similar to gadu-gadu)
    /// 
    /// Chat View is response for text chat with one selected user from contacts list - we can send and receive messages online.
    /// 
    /// </summary>
    public class MainController : Controller
    {

        private static string chatViewPath = ($"http://localhost:{BridgeSettings.WebPort}/Chat");

        /// <summary>
        /// 
        /// Method that opens a new conversation window with a selected user.
        /// 
        /// </summary>
        public static async Task<BrowserWindow> OpenChatDialog(int userId)
        {
            var browserWindow = await HelperMethods.GetFinwodWindowAsync($"Chat?user={userId}&");

            if (browserWindow != null){
                return browserWindow;
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
                    WebSecurity = false,
                    DevTools = false
                },

            }
            , $"{chatViewPath}?user={userId}&");

            browserWindow.RemoveMenu();
            browserWindow.OnReadyToShow += () => browserWindow.Show();
            
            return browserWindow;
        }


        /// <summary>
        /// 
        ///  Asynchronous method that enrolls most of the endpoints used in the view model.
        /// 
        /// </summary>
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

                        Electron.Dialog.ShowErrorBox(ex.Message, "Page context fatal error");
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
                        Electron.Dialog.ShowErrorBox(ex.Message, "Page context fatal error");
                    }

                    try
                    {
                        Int32 receiverId = output["receiverId"];
                        Int32 lastMessageId = output["lastMessageId"];

                        UserRequest ur = new UserRequest {
                            receiverId = receiverId,
                            lastMessageId = lastMessageId
                        };

                        List<Message> messages = await UserMainProfile.UserMessagesWithSelectedUserAsyncRequest(ur);

                        Electron.IpcMain.Send(mainWindow, returnChannel, (JsonConvert.SerializeObject(messages).ToString()));
                    }

                    catch (Exception ex)
                    {
                        HelperMethods.DisplayErrorMessage(ex, mainWindow, returnChannel, true);
                    }
                });



                Electron.IpcMain.On("send-message", async (args) =>
                {
                    string returnChannel = "send-message-reply";

                    var output = JsonConvert.DeserializeObject<Dictionary<string, string>>(args.ToString());
                    Int32 receiverUserId = Int32.Parse(output["receiverId"]);

                    string message = output["message"];

                    BrowserWindow mainWindow = null;
                    try
                    {
                        mainWindow = await HelperMethods.GetFinwodWindowAsync($"Chat?user={receiverUserId}");
                        string url = await mainWindow.WebContents.GetUrl();
                    }
                    catch (Exception ex)
                    {
                        Electron.Dialog.ShowErrorBox(ex.Message, "Page context fatal error");
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
                        UserRequest userRequest = new UserRequest {
                            username = args.ToString()
                        };

                        var messageRespond = await UserMainProfile.AddFriendAsyncRequest(userRequest);
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

                Electron.IpcMain.On("remove-friend", async (args) =>
                {

                    string returnChannel = "remove-friend-reply";
                    BrowserWindow mainWindow = await HelperMethods.GetFinwodWindowAsync("Main");

                    try
                    {
                        UserRequest userRequest = new UserRequest {
                            id = Int32.Parse(args.ToString())
                        };

                        var messageRespond = await UserMainProfile.RemoveFriendAsyncRequest(userRequest);
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


                Electron.IpcMain.On("block-friend", async (args) =>
                {

                    string returnChannel = "block-friend-reply";
                    BrowserWindow mainWindow = await HelperMethods.GetFinwodWindowAsync("Main");

                    try
                    {
                        UserRequest userRequest = new UserRequest {
                            id = Int32.Parse(args.ToString())
                        };

                        var messageRespond = await UserMainProfile.BlockFriendAsyncRequest(userRequest);
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


                Electron.IpcMain.On("update-description", async (args) =>
                {

                    string returnChannel = "update-description-reply";
                    BrowserWindow mainWindow = await HelperMethods.GetFinwodWindowAsync("Main");

                    try
                    {
                        UserRequest userRequest = new UserRequest {
                            description = args.ToString(),
                            status = UserMainProfile.UserProfile.status
                        };

                        var messageRespond = await UserMainProfile.UpdateUserAsyncRequest(userRequest);
                        UserMainProfile.UserProfile.description = args.ToString();

                        Electron.IpcMain.Send(mainWindow, returnChannel, (JsonConvert.SerializeObject(messageRespond).ToString()));

                    }
                    catch (Exception ex)
                    {
                        HelperMethods.DisplayErrorMessage(ex, mainWindow, returnChannel, false);
                    }
                });


                Electron.IpcMain.On("update-status", async (args) =>
                {

                    string returnChannel = "update-status-reply";

                    BrowserWindow mainWindow = await HelperMethods.GetFinwodWindowAsync("Main");

                    try
                    {

                        UserRequest userRequest = new UserRequest {
                            status = Int32.Parse(args.ToString())
                        };

                        var messageRespond = await UserMainProfile.UpdateUserAsyncRequest(userRequest);

                        UserMainProfile.UserProfile.status = Int32.Parse(args.ToString());

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