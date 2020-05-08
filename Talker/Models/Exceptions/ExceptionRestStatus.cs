using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Talker.Models.DTO
{
    public class ExceptionRestStatus: Exception
    {
        public string timestamp { get; set; }

        public Int32 status { get; set; }

        public string error { get; set; }

        public string message { get; set; }

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
