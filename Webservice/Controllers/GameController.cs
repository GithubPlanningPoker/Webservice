using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Webservice.DTOs;
using Webservice.Models;

namespace Webservice.Controllers
{
    [RoutePrefix("game")]
    public class GameController : ApiController
    {
        [Route("")]
        [HttpPost]
        public dynamic createGame([FromBody]NameDTO value)
        {
            var returnVal = new { Success = true,
                Id = getMD5(DateTime.Now.ToString()),
                HostId = getMD5(value.name + DateTime.Now.ToString()) };
            try
            {
                Database.CreateGame(returnVal.Id, returnVal.HostId);
            }
            catch (Exception e)
            {
                return new { Succes = false, e.Message };
            }            
            return returnVal;
        }

        [Route("{gameId:int}/user")]
        [HttpPost]
        public dynamic joinGame(string gameId, [FromBody]NameDTO value)
        {
            var returnval = new { Succes = true, UserId = getMD5(value.name) };
            try
            {
                Database.AddUser(gameId, returnval.UserId);
            }
            catch (Exception e)
            {
                return new { Succes = false, e.Message };
            }
            return returnval;
        }

        [Route("{gameId:int}/description")]
        [HttpGet]
        public dynamic getDescription(string gameId)
        {
            try
            {
                return new { Succes = true, Description = Database.GetDescription(gameId) };
            }
            catch (Exception e)
            {
                return new { Succes = false, e.Message };
            }
        }

        [Route("{gameId:int}/description")]
        [HttpPut]
        public dynamic updateDescription(string gameId, [FromBody]DescriptionUserIdDTO value)
        {
            try
            {
                Database.UpdateDescription(gameId, value.description);
                return new { Succes = true };
            }
            catch (Exception e)
            {
                return new { Succes = false, e.Message };
            }
        }

        [Route("{gameId:int}/description")]
        [HttpDelete]
        public dynamic clearDescription(string gameId)
        {
            try
            {
                Database.ClearDescription(gameId);
                return new { Succes = true };
            }
            catch (Exception e)
            {
                return new { Succes = false, e.Message };
            }
        }

        [Route("{gameId:int}/vote")]
        [HttpGet]
        public dynamic getVotes(string gameId)
        {
            try
            {                
                return new { Succes = true, Votes = Database.GetCurrentVotes(gameId) };
            }
            catch (Exception e)
            {
                return new { Succes = false, e.Message };
            }
        }

        [Route("{gameId:int}/vote")]
        [HttpDelete]
        public dynamic clearVotes(string gameId, [FromBody]UserIdDTO value)
        {
            try
            {
                Database.ClearVote(gameId, value.userId);
                return new { Succes = true };
            }
            catch (Exception e)
            {
                return new { Succes = false, e.Message };
            }
        }

        [Route("{gameId:int}/vote/{userId:int}")]
        [HttpPost]
        public dynamic addVote(string gameId, string userId, [FromBody]VoteDTO value)
        {
            try
            {
                Database.Vote(gameId, userId, value.vote);
                return new { Succes = true };
            }
            catch (Exception e)
            {
                return new { Succes = false, e.Message };
            }
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