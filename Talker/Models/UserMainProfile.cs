using ElectronNET.API.Entities;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Talker.Models.DTO;
using Talker.Models.DTO.Requests;

namespace Talker.Models
{
    public static class UserMainProfile
    {

        static RestHelper<Profile> SessionProfileRestHelper = new RestHelper<Profile>(true);

        static RestHelper<List<User>> SessionContactsRestHelper = new RestHelper<List<User>>(true);

        static RestHelper<List<Message>> MessagesRestHelper = new RestHelper<List<Message>>(true);

        static RestHelper<RestStatus> OtherOperationsRequest = new RestHelper<RestStatus>(true);

        static public Profile UserProfile
        {
            get; set;
        }

        public static async Task<Profile> UserProfileAsyncRequest()
        {
            UserProfile = await SessionProfileRestHelper.sendAsyncRequest("api/user/me");

            return UserProfile;
        }

        public static async Task<List<User>> UpdateUserContactsAsyncRequest()
        {
            UserProfile.contacts = await SessionContactsRestHelper.sendAsyncRequest("api/contacts/list");

            return UserProfile.contacts;
        }



        public static async Task<List<Message>> UserMessagesWithSelectedUserAsyncRequest(UserRequest userRequest)
        {

            foreach (User c in UserProfile.contacts)
            {
                if (c.id == userRequest.receiverId)
                {
                    c.massagesWithUser = await MessagesRestHelper.sendAsyncRequest("api/messages/list", userRequest);

                    return c.massagesWithUser;
                }
            }

            return null;

        }




        public static async Task<Message>  SendMessageToSelectedUserAsyncRequest(Message messageRequest)
        {

            User currentUserFromContatcts = null;

            foreach (User c in UserProfile.contacts)
            {
                if (c.id == messageRequest.receiverId)
                {
                    currentUserFromContatcts = c;
                }
            }


            List<Message> message = await MessagesRestHelper.sendAsyncRequest("api/messages/send", messageRequest);

            currentUserFromContatcts.massagesWithUser.Add(message[0]);

            return message[0];
        }

        public static async Task<RestStatus> AddFriendAsyncRequest(UserRequest userRequest)
        {
            RestStatus restResponse = await OtherOperationsRequest.sendAsyncRequest("api/contacts/add", userRequest);

            return restResponse;
        }


        public static async void ClearMessagesWithSelectedUserAsyncRequest(UserRequest userRequest)
        {

            foreach (User c in UserProfile.contacts)
            {
                if (c.id == userRequest.receiverId)
                {
                    c.massagesWithUser = null;
                }
            }
        }

    }
}
