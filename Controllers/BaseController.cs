using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackendService.Contexts;
using BackendService.Data;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BackendService.Controllers
{
    public class BaseController : ControllerBase
    {
        public ApplicationContext _db;
        private readonly ILogger<ApiController> _logger;

        /// <summary>
        /// Base controller to offload logging and context capability to another class
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public BaseController(ApplicationContext context, ILogger<ApiController> logger)
        {
            _db = context;
            _logger = logger;
            ApplyMigrations(_db);

        }
        public async Task<IActionResult> Json(int status, object json)
        {
            var payload = JsonSerializer.Serialize(json);
            var log = "StatusCode: " + status + "\nJson: " + payload;
            LogResponse(log);
            return await GetStatusResult(status, payload);
        }
        public async Task<IActionResult> Json(int status, object json, string message)
        {
            var payload = JsonSerializer.Serialize(json);
            var log = "StatusCode: " + status + "\nMessage: " + message == null ? "" : message + "\nJSON: " + payload;
            LogResponse(log);
            return await GetStatusResult(status, payload);
        }
        public async Task<IActionResult> GetStatusResult(int status, string payload)
        {
            switch (status)
            {
                case StatusCodes.Status200OK:
                    return Ok(payload);
                case StatusCodes.Status204NoContent:
                    return NoContent();
                case StatusCodes.Status500InternalServerError:
                    return StatusCode(500, payload);
                default:
                    return BadRequest(payload);
            }
        }
        public void LogResponse(string message)
        {
            _logger.LogInformation(message);
        }
        public void ApplyMigrations(ApplicationContext context)
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
                if (context.Database.EnsureCreated())
                {
                    throw new Exception("Unable to apply migration");
                }
            }
        }
    }
}
