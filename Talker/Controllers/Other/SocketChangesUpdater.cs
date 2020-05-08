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
    public class SocketChangesUpdater
    {

        BrowserWindow MainWindow;

        SockerServerHelper ChangesListener;

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

                    Console.WriteLine(echoResponse);

                    string[] status = echoResponse.Split(';');

                    int statusResponse = Int32.Parse(status[1]);

                    if (statusResponse == 1) // NEW_MESSAGE
                    {

                        Console.WriteLine($"RESPONSE: {statusResponse}");

                        string usersStr = status[2];

                        string[] users = usersStr.Split('-');

                        foreach (string userIdStr in users) 
                        {

                            int userId;
                            bool successConvertion = Int32.TryParse(userIdStr, out userId);

                            if (successConvertion && userId > 0)
                            {
                                Console.WriteLine(">>>>> "+ userId);

                                var bw = await HelperMethods.GetFinwodWindowAsync($"Chat?user={userId}&");

                                Console.WriteLine($"USER ID: {userId}");

                                if (bw == null)
                                {
                                    var chatWindow = await MainController.OpenChatDialog(userId);
                                    chatWindow.Focus();
                                }
                                else
                                {
                                    bw.Focus();


                                    UserRequest ur = new UserRequest { receiverId = userId, lastMessageId = 0 };

                                    List<Message> messages = await UserMainProfile.UserMessagesWithSelectedUserAsyncRequest(ur);


                                    Console.WriteLine(JsonConvert.SerializeObject(messages).ToString());

                                    Electron.IpcMain.Send(bw, "new-messages-replay", (JsonConvert.SerializeObject(messages).ToString()));


                                }

                            }
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
                catch (Exception) //:)
                {

                }

                HelperMethods.DisplayErrorMessage(ex, MainWindow, channel, false);
             }
        }
    }




}
