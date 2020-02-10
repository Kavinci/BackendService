using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BackendService.Data;
using BackendService.Contexts;
using BackendService.BusinessLogic;
using System.Text.Json;
using System.Net;

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
        public async Task<IActionResult> Init([FromBody] JsonModel.Request body)
        {
            var request = new Request()
            {
                RequestId = Guid.NewGuid(),
                Initiated = DateTime.Now,
                StatusCode = StatusType.INITIALIZED,
                Body = body.body
            };
            try
            {
                _db.Requests.Add(request);
                _db.SaveChanges();
            }
            catch
            {
                return await Json(500, "An error occured during initialization");
            }

            var payload = new JsonModel.Endpoint
            {
                body = body.body,
                callback = "https://" + Dns.GetHostName() + ":5001/callback/" + request.RequestId.ToString()
            };
            var response = Helpers.SendToService(request.Endpoint,"POST", payload);
            // check response for error codes
            return await Json(200, request.RequestId.ToString());
        }

        /// <summary>
        /// hostname/callback/{id} POST callback endpoint gets initial contact from service and syncs db record 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("/api/callback/{id}")]
        public async Task<IActionResult> PostCallback([FromRoute] Guid id, [FromBody] string body)
        {
            var request = _db.Requests.Find(id);
            if(request == null)
            {
                return await Json(404, "No record matching that Id");
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
                return await Json(500, "Error updating record");
            }

            return await Json(204, null);
        }

        /// <summary>
        /// hostname/callback/{id} PUT callback endpoint gets updates from service and syncs db record 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("/api/callback/{id}")]
        public async Task<IActionResult> PutCallback([FromRoute] Guid id, [FromBody] string body)
        {
            var obj = JsonSerializer.Deserialize<JsonModel.PUT>(body);
            var request = _db.Requests.Find(id);

            if (request == null)
            {
                return await Json(404, "No record matching that Id");
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
                return await Json(500, "Error updating record");
            }

            return await Json(204, null);
        }

        /// <summary>
        /// hostname/status/{id} GET status endpoint gets db info and returns
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/api/status/{id}")]
        public async Task<IActionResult> Status([FromRoute] string id)
        {
            var request = _db.Requests.Find(id);

            if(request == null)
            {
                return await Json(404, "No record matching that Id");
            }

            var res = new JsonModel.GET
            {
                status = request.Status,
                detail = request.Details,
                body = request.Body,
                created = request.Initiated,
                lastUpdated = request.Updated
            };

            return await Json(200, JsonSerializer.Serialize(res));
        }
    }
}
