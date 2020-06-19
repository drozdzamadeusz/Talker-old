using System.Collections.Generic;
using System.Threading.Tasks;

using Talker.Models.DTO;
using Talker.Models.DTO.Requests;

namespace Talker.Models
{

    /// <summary>
    /// 
    /// Stores information about logged user and list of his contacts. 
    /// 
    /// </summary>
    public static class UserMainProfile
    {

        /// <summary>
        /// 
        /// Instance of RestHelper used for getting informations about currently logged user
        /// Each request sent by this instance has a bearer token authorization header.
        /// 
        /// </summary>
        static readonly RestHelper<Profile> SessionProfileRestHelper = new RestHelper<Profile>(true);

        /// <summary>
        /// 
        /// Instance of RestHelper used for getting user's contacts list.
        /// Each request sent by this instance has a bearer token authorization header.
        /// 
        /// </summary>
        static readonly RestHelper<List<User>> SessionContactsRestHelper = new RestHelper<List<User>>(true);

        /// <summary>
        /// 
        /// Instance of RestHelper used for getting user's messages list.
        /// Each request sent by this instance has a bearer token authorization header.
        /// 
        /// </summary>
        static readonly RestHelper<List<Message>> MessagesRestHelper = new RestHelper<List<Message>>(true);


        /// <summary>
        /// 
        /// Instance of RestHelper used for sendting data of other type to the rest api.
        /// Each request sent by this instance has a bearer token authorization header.
        /// 
        /// </summary>
        static readonly RestHelper<RestStatus> OtherOperationsRequest = new RestHelper<RestStatus>(true);


        /// <summary>
        /// 
        /// User Profile DTO
        /// Contains informations about curentylly logged user - such as login or current description.
        /// 
        /// </summary>
        static public Profile UserProfile
        {
            get; set;
        }


        /// <summary>
        /// 
        /// Asynchronous method used to retrieve data of the currently logged in user
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// Profile DTO (all data about one main user)
        /// 
        /// </returns>
        public static async Task<Profile> UserProfileAsyncRequest()
        {
            UserProfile = await SessionProfileRestHelper.sendAsyncRequest("api/user/me");

            return UserProfile;
        }


        /// <summary>
        /// 
        /// Asynchronous method used to get contacts of the currently logged in user
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// List of User DTOs (contacts of main user)
        /// 
        /// </returns>
        public static async Task<List<User>> UpdateUserContactsAsyncRequest()
        {
            UserProfile.contacts = await SessionContactsRestHelper.sendAsyncRequest("api/contacts/list");

            return UserProfile.contacts;
        }


        /// <summary>
        /// 
        /// Asynchronous method used to get list of messages of currently logged in user with other selected user
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// List of Message DTOs (messages of main user with other user)
        /// 
        /// </returns>
        /// <param name="userRequest">UserRequest object - recipient user id</param>
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


        /// <summary>
        /// 
        /// Asynchronous method used to send a message to the selected user.
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// List of Message DTOs (messages of main user with other user)
        /// 
        /// </returns>
        /// <param name="messageRequest">Message DTO - message that has to be send to selected user.</param>
        public static async Task<Message> SendMessageToSelectedUserAsyncRequest(Message messageRequest)
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


        /// <summary>
        /// 
        /// Asynchronous method used to add a new user to user's contacts list.
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// Status response whether the process successfully
        /// 
        /// </returns>
        /// <param name="userRequest">UserRequest object - selected user that has to be added to the contacts list</param>
        public static async Task<RestStatus> AddFriendAsyncRequest(UserRequest userRequest)
        {
            RestStatus restResponse = await OtherOperationsRequest.sendAsyncRequest("api/contacts/add", userRequest);

            return restResponse;
        }


        /// <summary>
        /// 
        /// Asynchronous method used to add remove selected user form user's contacts list.
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// Status response whether the process successfully
        /// 
        /// </returns>
        /// <param name="userRequest">UserRequest object - selected user that has to be removed from the contacts list</param>
        public static async Task<RestStatus> RemoveFriendAsyncRequest(UserRequest userRequest)
        {
            RestStatus restResponse = await OtherOperationsRequest.sendAsyncRequest("api/contacts/remove", userRequest);

            return restResponse;
        }


        /// <summary>
        /// 
        /// Asynchronous method used to block selected user form user's contacts list.
        /// Blocked user will not be able to send messages to the main user.
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// Status response whether the process successfully
        /// 
        /// </returns>
        /// <param name="userRequest">UserRequest object - selected user that has to be blocked</param>
        public static async Task<RestStatus> BlockFriendAsyncRequest(UserRequest userRequest)
        {
            RestStatus restResponse = await OtherOperationsRequest.sendAsyncRequest("api/contacts/block", userRequest);

            return restResponse;
        }


        /// <summary>
        /// 
        /// Asynchronous method used to update main user's status
        /// Eg. user current picture,  description or availability status.
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// Status response whether the process successfully
        /// 
        /// </returns>
        /// <param name="userRequest">UserRequest object - selected user that has to be blocked</param>
        public static async Task<RestStatus> UpdateUserAsyncRequest(UserRequest userRequest)
        {
            RestStatus restResponse = await OtherOperationsRequest.sendAsyncRequest("api/user/update", userRequest);

            return restResponse;
        }


        /// <summary>
        /// 
        /// Helper method that clears all messages with slected user.
        /// 
        /// </summary>
        /// <param name="userRequest">UserRequest object - selected user that has to be blocked</param>
        public static void ClearMessagesWithSelectedUserAsyncRequest(UserRequest userRequest)
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
