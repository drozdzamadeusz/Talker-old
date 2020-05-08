using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talker.Models.DTO.Requests
{
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
