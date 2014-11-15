using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webservice.Response
{
    public class GetUsersResponse
    {
        public List<GetUserResponse> users { get; set; }

        public GetUsersResponse()
        {
            users = new List<GetUserResponse>();
        }
    }
}