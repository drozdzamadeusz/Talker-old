using System;

namespace Talker.Models
{

    /// <summary>
    /// 
    /// Default (correct) REST API response
    /// This DTS's is a mirror image of DTS's stored on the server-site
    /// For this reason, variable names are lowercase
    /// 
    /// </summary>
    public class RestStatus
    {

        /// <summary>
        /// 
        /// If response is OK the this variable is empty
        /// 
        /// </summary>
        public string errortype { get; set; }

        /// <summary>
        /// 
        /// Current server timestrap.
        /// 
        /// </summary>
        public string timestamp { get; set; }


        /// <summary>
        /// 
        /// Http status (200)
        /// 
        /// </summary>
        public Int32 status { get; set; }


        /// <summary>
        /// 
        /// If response is OK that variable is used as response header information
        /// 
        /// </summary>
        public string error { get; set; }


        /// <summary>
        /// 
        /// If response is OK that variable is used as response information
        /// 
        /// </summary>
        public string message { get; set; }


        /// <summary>
        /// 
        /// Path on the server.
        /// 
        /// </summary>
        public string path { get; set; }
    }
}
