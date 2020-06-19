using System;

namespace Talker.Models.DTO
{
    /// <summary>
    /// 
    /// User's DTO contains bearer token information of currently logged user
    /// This DTS's is a mirror image of DTS's stored on the server-site
    /// For this reason, variable names are lowercase
    /// 
    /// </summary>
    public class Token
    {
        public string token { get; set; }
    }
}
