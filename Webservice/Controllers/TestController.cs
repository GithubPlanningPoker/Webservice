using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Webservice.Models;

namespace Webservice.Controllers
{
    public class TestController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            Database.CreateGame("abekat", "osten");
            return new string[] { "value1", "value2" };
        }        
    }
}