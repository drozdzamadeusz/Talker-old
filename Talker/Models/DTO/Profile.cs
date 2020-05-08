using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talker.Models.DTO
{
    public class Profile : User
    {
        public List<User> contacts { get; set; }
    }
}
