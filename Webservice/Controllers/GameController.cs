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
        // POST api/<controller>
        public Game Get([FromBody]String value)
        {
            Game game = new Game
            {
                Id = getMD5(DateTime.Now.ToString()),
                HostId = getMD5(value + DateTime.Now.ToString())
            };

            return game;
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