using System.Collections.Generic;

namespace Talker.Models.DTO
{


    /// <summary>
    /// 
    /// User's DTO stares information about user and list of his contacts. 
    /// This DTS's is a mirror image of DTS's stored on the server-site
    /// For this reason, variable names are lowercase
    /// 
    /// </summary>
    public class Profile : User
    {
        public List<User> contacts { get; set; }
    }
}
