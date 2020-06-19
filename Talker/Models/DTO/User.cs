using System;
using System.Collections.Generic;

namespace Talker.Models.DTO
{
    /// <summary>
    /// 
    /// User's DTO contains information about one user in the system
    /// This DTS's is a mirror image of DTS's stored on the server-site
    /// For this reason, variable names are lowercase
    /// 
    /// </summary>
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
