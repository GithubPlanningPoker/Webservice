﻿using System;
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
            string gameIdHash = getMD5(DateTime.Now.Ticks.ToString());
            while (Database.GameExists(gameIdHash))
            {
                gameIdHash = getMD5(DateTime.Now.Ticks.ToString());
            }
            var returnVal = new { success = true,
                gameid = gameIdHash,
                userid = getMD5(value.name + DateTime.Now.ToString()) };
            try
            {
                Database.CreateGame(returnVal.gameid, returnVal.userid);
            }
            catch (Exception e)
            {
                return new { success = false, message = e.Message };
            }            
            return returnVal;
        }

        [Route("{gameId}/user")]
        [HttpPost]
        public dynamic joinGame(string gameId, [FromBody]NameDTO value)
        {
            var returnval = new { success = true, userid = getMD5(value.name) };
            try
            {
                Database.AddUser(gameId, returnval.userid);
            }
            catch (Exception e)
            {
                return new { success = false, message = e.Message };
            }
            return returnval;
        }

        [Route("{gameId}/description")]
        [HttpGet]
        public dynamic getDescription(string gameId)
        {
            try
            {
                return new { success = true, description = Database.GetDescription(gameId) };
            }
            catch (Exception e)
            {
                return new { success = false, message = e.Message };
            }
        }

        [Route("{gameId}/description")]
        [HttpPut]
        public dynamic updateDescription(string gameId, [FromBody]DescriptionUserIdDTO value)
        {
            try
            {
                Database.UpdateDescription(gameId, value.description);
                return new { success = true };
            }
            catch (Exception e)
            {
                return new { success = false, message = e.Message };
            }
        }

        [Route("{gameId}/description")]
        [HttpDelete]
        public dynamic clearDescription(string gameId)
        {
            try
            {
                Database.ClearDescription(gameId);
                return new { success = true };
            }
            catch (Exception e)
            {
                return new { success = false, message = e.Message };
            }
        }

        [Route("{gameId}/vote")]
        [HttpGet]
        public dynamic getVotes(string gameId)
        {
            try
            {                
                return new { success = true, votes = Database.GetCurrentVotes(gameId) };
            }
            catch (Exception e)
            {
                return new { success = false, message = e.Message };
            }
        }

        [Route("{gameId}/vote")]
        [HttpDelete]
        public dynamic clearVotes(string gameId, [FromBody]UserIdDTO value)
        {
            try
            {
                Database.ClearVote(gameId, value.userId);
                return new { success = true };
            }
            catch (Exception e)
            {
                return new { success = false, message = e.Message };
            }
        }

        [Route("{gameId}/vote/{userId}")]
        [HttpPost]
        public dynamic addVote(string gameId, string userId, [FromBody]VoteDTO value)
        {
            try
            {
                Database.Vote(gameId, userId, value.vote);
                return new { success = true };
            }
            catch (Exception e)
            {
                return new { success = false, message = e.Message };
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