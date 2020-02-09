using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BackendService.Data;
using BackendService.Contexts;

namespace BackendService.Controllers
{
    public class ApiController : BaseController
    {
        /// <summary>
        /// Controller for the API endpoints with some logging
        /// </summary>
        /// <param name="logger"></param>
        public ApiController(ApplicationContext context, ILogger<ApiController> logger) : base(context, logger) { }

        [HttpPost]
        [Route("request")]
        public string Init([FromBody] object body)
        { 
            var request = new Request()
            {
                RequestId = Guid.NewGuid(),
                Initiated = DateTime.Now,
                StatusCode = StatusType.INITIALIZED
            };
            try
            {
                _db.Requests.Add(request);
                _db.SaveChanges();
            }
            catch
            {
                var err = Respond(500, "An error occured during initialization", null);
                return err.ToString();
            }
            // Send request to third party
            return request.RequestId.ToString();
        }

        [HttpPost]
        [Route("callback/{id}")]
        public ActionResult PostCallback(Guid id, [FromBody] string body)
        {
            var request = _db.Requests.Find(id);
            if(request == null)
            {
                return Respond(404, "No record matching that Id", null);
            }
            request.Status = body;
            try
            {
                _db.Requests.Update(request);
                _db.SaveChanges();
            }
            catch
            {
                return Respond(500, "Error updating record", null);
            }
            return Respond(204, null, null);
        }

        [HttpPut]
        [Route("callback/{id}")]
        public ActionResult PutCallback([FromBody] object obj)
        {
            return Respond(204, null, null);
        }

        [HttpGet]
        [Route("status/{id}")]
        public ActionResult Status(string id)
        {
            var request = _db.Requests.Find(id);
            if(request == null)
            {
                return Respond(404, "No record matching that Id", null);
            }
            return Respond(200, null, request.Status);
        }
    }
}
