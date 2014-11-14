using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webservice.Response
{
    public class GetUserResponse
    {
        public string username { get; set; }
        public bool voted { get; set; }
        public string vote { get; set; }

        public GetUserResponse() { }
    }
}