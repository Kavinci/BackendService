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
        public ApiController(ApplicationContext context, ILogger<ApiController> logger) : base(context, logger) {      }

        [HttpGet]
        public string IsRunning()
        {
            return "App is running";
        }

        [HttpGet("{id}")]
        public Request Status(string id)
        {
            return new Request();
        }
    }
}
