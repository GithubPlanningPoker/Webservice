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
    [RoutePrefix("api/game")]
    public class GameController : ApiController
    {
        [Route("")]
        [HttpPost]
        public dynamic createGame([FromBody]String value)
        {
            var returnVal = new { Success = true,
                Id = getMD5(DateTime.Now.ToString()),
                HostId = getMD5(value + DateTime.Now.ToString()) };

            return returnVal;
        }

        [Route("{gameId:int}/user")]
        [HttpPost]
        public dynamic joinGame(int gameId, [FromBody]String value)
        {
            throw new NotImplementedException();
        }

        [Route("{gameId:int}/description")]
        [HttpGet]
        public dynamic getDescription(int gameId)
        {
            throw new NotImplementedException();
        }

        [Route("{gameId:int}/description")]
        [HttpPut]
        public dynamic updateDescription(int gameId, [FromBody]List<String> value)
        {
            String description = value[0]; //TODO not entirely sure that this will work
            String userId = value[1];

            throw new NotImplementedException();
        }

        [Route("{gameId:int}/description")]
        [HttpDelete]
        public dynamic clearDescription(int gameId)
        {
            throw new NotImplementedException();
        }

        [Route("{gameId:int}/vote")]
        [HttpGet]
        public dynamic getVotes(int gameId)
        {
            throw new NotImplementedException();
        }

        [Route("{gameId:int}/vote")]
        [HttpDelete]
        public dynamic clearVotes(int gameId, [FromBody]String value)
        {
            throw new NotImplementedException();
        }

        [Route("{gameId:int}/vote/{userId:int}")]
        [HttpPost]
        public dynamic addVote(int gameId, int userId)
        {
            throw new NotImplementedException();
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