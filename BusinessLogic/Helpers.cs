using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackendService.BusinessLogic
{
    public class Helpers
    {

        /// <summary>
        /// Sends data to an endpoint with specified method and URI.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string SendToService(string uri, string method, object json)
        {
            // Sending code for an actual endpoint
            //HttpWebRequest webRequest = WebRequest.CreateHttp(uri);
            //webRequest.ContentType = "application/json";
            //webRequest.Method = method;

            //using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(JsonSerializer.Serialize(json));
            //}

            //var response = webRequest.GetResponse();
            //using(var streamReader = new StreamReader(response.GetResponseStream()))
            //{
            //    return streamReader.ReadToEnd();
            //}
            return "";
        }
    }
}
