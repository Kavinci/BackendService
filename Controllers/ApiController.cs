using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BackendService.Data;
using BackendService.Contexts;
using BackendService.BusinessLogic;

namespace BackendService.Controllers
{
    public class ApiController : BaseController
    {
        /// <summary>
        /// Controller for the API endpoints with some logging
        /// </summary>
        /// <param name="logger"></param>
        public ApiController(ApplicationContext context, ILogger<ApiController> logger) : base(context, logger) { }
        
        [HttpGet("/api/isrunning")]
        public string IsRunning()
        {
            return "App is running";
        }
        /// <summary>
        /// hostname/request POST request endpoint initializes communication with service and creates a db record  
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("/api/request")]
        public string Init([FromBody] string body)
        {
            var obj = JsonSerializer.Deserialize<JsonModel.PUT>(body);
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

            var payload = new JsonModel.Endpoint
            {
                body = body,
                callback = "hostname/callback/" + request.RequestId.ToString()
            };
            var json = JsonSerializer.Serialize(payload);
            var response = Helpers.Send(json);
            // check response for error codes
            return request.RequestId.ToString();
        }

        /// <summary>
        /// hostname/callback/{id} POST callback endpoint gets initial contact from service and syncs db record 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("/api/callback/{id}")]
        public ActionResult PostCallback([FromRoute] Guid id, [FromBody] string body)
        {
            var request = _db.Requests.Find(id);
            if(request == null)
            {
                return Respond(404, "No record matching that Id", null);
            }

            request.Status = body;
            request.Updated = DateTime.Now;

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

        /// <summary>
        /// hostname/callback/{id} PUT callback endpoint gets updates from service and syncs db record 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("/api/callback/{id}")]
        public ActionResult PutCallback([FromRoute] Guid id, [FromBody] string body)
        {
            var obj = JsonSerializer.Deserialize<JsonModel.PUT>(body);
            var request = _db.Requests.Find(id);

            if (request == null)
            {
                return Respond(404, "No record matching that Id", null);
            }

            request.Status = obj.status.ToString();
            request.Details = obj.detail.ToString();
            request.Updated = DateTime.Now;

            if(request.StatusCode == StatusType.COMPLETED)
            {
                request.Completed = DateTime.Now;
            }

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

        /// <summary>
        /// hostname/status/{id} GET status endpoint gets db info and returns
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/status/{id}")]
        public ActionResult Status([FromRoute] string id)
        {
            var request = _db.Requests.Find(id);

            if(request == null)
            {
                return Respond(404, "No record matching that Id", null);
            }

            var res = new JsonModel.GET
            {
                status = request.Status,
                detail = request.Details,
                body = request.Body,
                created = request.Initiated,
                lastUpdated = request.Updated
            };

            return Respond(200, null, JsonSerializer.Serialize(res));
        }
    }
}
