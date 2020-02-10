using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendService.Data
{
    public class JsonModel
    {
        public class Request
        {
            public string body { get; set; }
        }
        public class Endpoint
        {
            public string body { get; set; }
            public string callback { get; set; }
        }
        public class PUT
        {
            public string status { get; set; }
            public string detail { get; set; }
        }
        public class GET
        {
            public string status { get; set; }
            public string detail { get; set; }
            public string body { get; set; }
            public DateTime created { get; set; }
            public DateTime lastUpdated { get; set; }

        }
    }
}
