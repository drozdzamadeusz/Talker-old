using ElectronNET.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Talker.Models;
using Talker.Models.DTO;
using Talker.Models.DTO.Requests;

namespace Talker.Controllers.Other
{

    /// <summary>
    /// 
    /// Heartbeat client witch listens in loop for changes that are related with the user using tcp connection
    /// such as new messages or changes of friends status.
    /// These messages don’t need to be encrypted because they don't contain any crucial data - they are only used as
    /// a trigger for encrypted rest api request. eg. pull new messages from user id 5.
    /// 
    /// For example if our user id is equal to 4 the request to tcp heartbeat server will look like this:
    /// ";4;1590240978" were "4" is user id and long number is current user 's current timestrap.
    /// If we have incoming messages from for example user id 2 and user id 7 echo response will be as follows:
    /// "1;-2-7-" were "1" means that we have new incoming message and -2-7- indicates sender users ids.
    /// But If we did not received any new updates, response will be simply ";0;"
    /// 
    /// </summary>
    public class SocketChangesUpdater
    {

        private BrowserWindow MainWindow;
        private SockerServerHelper ChangesListener;

        public SocketChangesUpdater(BrowserWindow mainWindow)
        {
            this.MainWindow = mainWindow;
            ChangesListener = new SockerServerHelper(UserMainProfile.UserProfile.id);
        }

        public async Task RunEchoSocketChangesUpdaterAsync()
        {

            string channel = "echo-socket-updater-replay";

            try
            {

                if (ChangesListener.EstablishConnection())
                {

                    string echoResponse = "";
                    echoResponse = ChangesListener.CheckNewChanges();

                    string[] status = echoResponse.Split(';');
                    int statusResponse = Int32.Parse(status[1]);

                    if (statusResponse == 1) // NEW_MESSAGE
                    {
                        Console.WriteLine($"RESPONSE: {statusResponse}");

                        string usersStr = status[2];
                        string[] users = usersStr.Split('-');

                        foreach (string userIdStr in users)
                        {
                            Int32 userId;
                            bool successConvertion = Int32.TryParse(userIdStr, out userId);

                            if (successConvertion && userId > 0)
                            {

                                var bw = await HelperMethods.GetFinwodWindowAsync($"Chat?user={userId}&");

                                if (bw == null)
                                {
                                    var chatWindow = await MainController.OpenChatDialog(userId);
                                    chatWindow.Focus();
                                }
                                else
                                {
                                    bw.Focus();

                                    UserRequest ur = new UserRequest {
                                        receiverId = userId,
                                        lastMessageId = 0
                                    };

                                    List<Message> messages = await UserMainProfile.UserMessagesWithSelectedUserAsyncRequest(ur);
                                    Electron.IpcMain.Send(bw, "new-messages-replay", (JsonConvert.SerializeObject(messages).ToString()));
                                }

                            }
                        }
                    }
                    else if (statusResponse == 2) // FRIENDS UPDATE
                    {
                        BrowserWindow mainWindow = await HelperMethods.GetFinwodWindowAsync("Main");
                        await UserMainProfile.UpdateUserContactsAsyncRequest();

                        if (mainWindow != null)
                        {
                            Electron.IpcMain.Send(mainWindow, "user-profile-clear-contacts-list", "");
                            Electron.IpcMain.Send(mainWindow, "user-profile-reply", (JsonConvert.SerializeObject(UserMainProfile.UserProfile).ToString()));
                        }
                    }
                    else if (statusResponse == 4 || statusResponse == 5)// NEW FRIEND OR FRIEND REMOVED 
                    {
                        BrowserWindow mainWindow = await HelperMethods.GetFinwodWindowAsync("Main");
                        await UserMainProfile.UserProfileAsyncRequest();

                        if (mainWindow != null)
                        {
                            Electron.IpcMain.Send(mainWindow, "user-profile-reply", (JsonConvert.SerializeObject(UserMainProfile.UserProfile).ToString()));
                        }
                    }
                    Electron.IpcMain.Send(MainWindow, channel, JsonConvert.SerializeObject(new RestStatus { message = "" }).ToString());
                }

            }
            catch (Exception ex)
            {

                try
                {
                    ChangesListener.Disconnect();
                }
                catch (Exception ex1)
                {
                    throw ex1;
                }

                HelperMethods.DisplayErrorMessage(ex, MainWindow, channel, false);
             }
        }
    }




}
