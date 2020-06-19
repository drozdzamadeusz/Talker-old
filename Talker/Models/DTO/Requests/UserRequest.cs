using System;

namespace Talker.Models.DTO.Requests
{


    /// <summary>
    /// 
    /// This DTO's is used to send information about user to server
    /// Eg. If uses is logging into the system object of that class is sent to api with username and password fulfilled
    /// This DTS's is a mirror image of DTS's stored on the server-site
    /// For this reason, variable names are lowercase
    /// 
    /// </summary>
    public class UserRequest
    {
        public Int32 id { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public string email { get; set; }
        public string image { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public Int32 lastMessageId { get; set; }

        public Int32 receiverId { get; set; }

        public Int32 status { get; set; }
        public string description { get; set; }
    }
}
