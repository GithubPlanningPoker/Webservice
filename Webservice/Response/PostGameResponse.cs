using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webservice.Response
{
    public class PostGameResponse
    {
        public string gameId { get; set; }
        public string userId { get; set; }

        public PostGameResponse() { }
    }
}