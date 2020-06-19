using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Talker.Models.DTO
{

    /// <summary>
    /// 
    /// Exception that is thrown when API returns invalid results status
    /// Eg. 404 instead of 200
    /// 
    /// </summary>
    public class ExceptionRestStatus: Exception
    {
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


        public override string Message
        {
            get
            {
                return message;
            }
        }


        [JsonIgnore]
        [IgnoreDataMember]
        public override IDictionary Data { get; }

        public RestStatus toRestStatus()
        {
            return new RestStatus {
                errortype = "rest",
                timestamp = this.timestamp,
                error = this.error,
                message = this.message,
                path = this.path,
                status = this.status
            };
        }

    }
}