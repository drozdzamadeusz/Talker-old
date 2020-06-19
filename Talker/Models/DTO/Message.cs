using System;
using Talker.Models.DTO.Requests;

namespace Talker.Models.DTO
{

    /// <summary>
    /// 
    /// User's DTO contains information about one single message in the system.
    /// This DTS's is a mirror image of DTS's stored on the server-site
    /// For this reason, variable names are lowercase
    /// 
    /// </summary>
    public class Message
    {

        public Int32 messageId { get; set; }
        public Int32 senderId { get; set; }
        public Int32 receiverId { get; set; }
        public string message { get; set; }
        public Int32 seen { get; set; }
        public string seenTime { get; set; }
        public string addedTime { get; set; }

        public static implicit operator Message(UserRequest v)
        {
            throw new NotImplementedException();
        }
    }
}
