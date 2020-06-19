using System;

namespace Talker.Models
{

    /// <summary>
    /// 
    /// Error handling error pages occured on view layer.
    /// 
    /// </summary>
    public class ErrorViewModel
    {

        /// <summary>
        /// 
        /// Improper page request identifier 
        /// 
        /// </summary>
        public string RequestId { get; set; }


        /// <summary>
        /// 
        /// Wheter page request identifier has to be shown on view layer.
        /// 
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}