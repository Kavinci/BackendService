using System;
using System.ComponentModel.DataAnnotations;

namespace BackendService.Data
{
    public class Request
    {
        public Guid RequestId { get; set; }
        public DateTime Initiated { get; set; }
        public DateTime Completed { get; set; }
        public StatusType Status { get; set; }
    }

    public enum StatusType
    {
        STARTED,
        PROCESSED,
        COMPLETED,
        ERROR
    }
}
