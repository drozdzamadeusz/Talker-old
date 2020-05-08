using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talker.Models.DTO
{
    public class User
    {
        public Int32 id { get; set; }
        public string username { get; set; }


        public string email { get; set; }
        public string image { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }


        public Int32 status { get; set; }
        public string description { get; set; }
        public Int32 unreadMessages { get; set; }
        public Int32 addedByUser { get; set; }

        public List<Message> massagesWithUser { get; set; }

    }
}
