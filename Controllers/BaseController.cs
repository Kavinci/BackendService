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

namespace BackendService.Controllers
{
    [Route("api/[action]")]
    [ApiController]
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
        }

        public ActionResult Respond(int status, string? message, object? obj)
        {
            var log = "StatusCode: " + status + "\nMessage: " + message + "\nResponseObj: " + obj.ToString();
            switch (status)
            {
                case StatusCodes.Status200OK:
                    _logger.LogInformation(log);
                    return Ok(message ?? obj);
                case StatusCodes.Status204NoContent:
                    _logger.LogInformation(log);
                    return NoContent();
                case StatusCodes.Status500InternalServerError:
                    _logger.LogInformation(log);
                    return StatusCode(500);
                default:
                    _logger.LogInformation(log);
                    return BadRequest();
            }
        }
    }
}
