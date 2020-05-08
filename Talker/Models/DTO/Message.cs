using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talker.Models.DTO.Requests;

namespace Talker.Models.DTO
{
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
