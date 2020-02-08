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
        public DateTime Initiated { get; set; }
        public DateTime Completed { get; set; }
        public StatusType Status { get; set; }
    }

    /// <summary>
    /// Status enum for the API
    /// </summary>
    public enum StatusType
    {
        STARTED,
        PROCESSED,
        COMPLETED,
        ERROR
    }
}
