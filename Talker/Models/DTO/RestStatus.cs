using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Talker.Models
{
    public class RestStatus
    {

        public string errortype { get; set; }

        public string timestamp { get; set; }

        public Int32 status { get; set; }

        public string error { get; set; }

        public string message { get; set; }

        public string path { get; set; }
    }
}
