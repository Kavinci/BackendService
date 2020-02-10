using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BackendService.BusinessLogic
{
    public class Helpers
    {
        public static string Send(string json)
        {
            HttpWebRequest webRequest = WebRequest.CreateHttp("https://urldefense.proofpoint.com/v2/url?u=http-3A__example.com_request&d=DwIGAg&c=iWzD8gpC8N4xSkOHZBDmCw&r=R0U6eziUSfkIiSy6xlVVHEbyT-5CVX85B2177L6G3Po&m=yeOGbdLEit9cyYWgLXxv5PRcMgRiallgPowRbt59hFw&s=lZ8qcf2Nw6VP2qI311Xp3wnZgZDhuaIrUg7krpQgTr4&e=");
            webRequest.ContentType = "application/json";
            webRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            var response = webRequest.GetResponse();
            using(var streamReader = new StreamReader(response.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
