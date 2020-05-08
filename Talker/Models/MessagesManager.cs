using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talker.Models.DTO;

namespace Talker.Models.Exceptions
{
    public static class MessagesManager
    {

        static RestHelper<Message> MessageRestHelper = new RestHelper<Message>();


        static public List<User> UserContacts
        {
            get; set;
        }


    }
}
