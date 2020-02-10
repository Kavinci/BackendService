using System;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Data
{

    /// <summary>
    /// Request model for the API
    /// </summary>
    public class Request
    {
        public Guid RequestId { get; set; }
        public string Endpoint = "https://urldefense.proofpoint.com/v2/url?u=http-3A__example.com_request&d=DwIGAg&c=iWzD8gpC8N4xSkOHZBDmCw&r=R0U6eziUSfkIiSy6xlVVHEbyT-5CVX85B2177L6G3Po&m=yeOGbdLEit9cyYWgLXxv5PRcMgRiallgPowRbt59hFw&s=lZ8qcf2Nw6VP2qI311Xp3wnZgZDhuaIrUg7krpQgTr4&e=";
        public DateTime Initiated { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Completed { get; set; }
        public StatusType StatusCode { get; set; }
        public string Status { get 
            {
                switch (StatusCode)
                {
                    case StatusType.COMPLETED:
                        return "COMPLETED";
                    case StatusType.PROCESSED:
                        return "PROCESSED";
                    case StatusType.STARTED:
                        return "STARTED";
                    case StatusType.ERROR:
                        return "ERROR";
                    default:
                        return "INITIALIZED";
                }
            }
            set
            {
                switch (value)
                {
                    case "COMPLETED":
                        StatusCode = StatusType.COMPLETED;
                        break;
                    case "PROCESSED":
                        StatusCode = StatusType.PROCESSED;
                        break;
                    case "STARTED":
                        StatusCode = StatusType.STARTED;
                        break;
                    case "ERROR":
                        StatusCode = StatusType.ERROR;
                        break;
                    default:
                        StatusCode = StatusType.INITIALIZED;
                        break;
                }
            }
        }
        public string Details { get; set; }
        public string Body { get; set; }
    }

    /// <summary>
    /// Status enum for the API
    /// </summary>
    public enum StatusType
    {
        INITIALIZED,
        STARTED,
        PROCESSED,
        COMPLETED,
        ERROR
    }
}
