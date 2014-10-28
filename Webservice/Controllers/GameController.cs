using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Webservice.Models;

namespace Webservice.Controllers
{
    public class GameController : ApiController
    {
        // POST api/game
        public dynamic Post([FromBody]String value)
        {
            var returnVal = new { Success = true,
                Id = getMD5(DateTime.Now.ToString()),
                HostId = getMD5(value + DateTime.Now.ToString()) };

            return returnVal;
        }

        private String getMD5(String input)
        {
            MD5 md5Hash = MD5.Create();

            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sb.Append(data[i].ToString("x2"));

            return sb.ToString();
        }
    }
}