using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webservice.Response
{
    public class GetUsersResponse
    {
        public bool allVoted { get; set; }
        public List<GetUserResponse> users { get; set; }

        public GetUsersResponse() { }
    }
}