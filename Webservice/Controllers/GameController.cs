using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Webservice.DTOs;
using Webservice.Maintenance;
using Webservice.Models;

namespace Webservice.Controllers
{
    /// <summary>
    /// A Planning Poker game.
    /// </summary>
    [RoutePrefix("game")]
    public class GameController : ApiController
    {
        /// <summary>
        /// Create a new game, with the provided username as host.
        /// </summary>
        /// <param name="value">The host name.</param>
        /// <returns>The gameId and the host's userId.</returns>
        [Route("")]
        [HttpPost]
        public dynamic createGame([FromBody]NameDTO value)
        {
            FileCleaner.DeleteFiles(24); //Delete files which were not modified in the past 24 hours.
            string gameIdHash = getMD5(DateTime.Now.Ticks.ToString());
            while (Database.GameExists(gameIdHash))
            {
                gameIdHash = getMD5(DateTime.Now.Ticks.ToString());
            }
            var returnVal = new
            {
                success = true,
                gameid = gameIdHash,
                userid = getMD5(value.name + DateTime.Now.ToString())
            };
            try
            {
                Database.CreateGame(returnVal.gameid, returnVal.userid, value.name);
            }
            catch (Exception e)
            {
                return new { success = false, message = e.Message };
            }
            return returnVal;
        }

        /// <summary>
        /// Gets all game information
        /// </summary>
        /// <param name="gameId">The gameId.</param>
        /// <returns>The game's title, description, and votes.</returns>
        [Route("{gameId}")]
        [HttpGet]
        public dynamic getGame(string gameId)
        {
            try
            {
                return new
                {
                    success = true,
                    host = Database.GetHost(gameId)
                };
            }
            catch (Exception e)
            {
                return new { success = false, message = e.Message };
            }
        }

        /// <summary>
        /// Join a game with the provided username.
        /// </summary>
        /// <param name="gameId">The gameId.</param>
        /// <param name="value">The username.</param>
        /// <returns></returns>
        [Route("{gameId}/user")]
        [HttpPost]
        public dynamic joinGame(string gameId, [FromBody]NameDTO value)
        {
            var returnval = new { success = true, userid = getMD5(value.name) };
            try
            {
                Database.AddUser(gameId, returnval.userid, value.name);
            }
            catch (Exception e)
            {
                return new { success = false, message = e.Message };
            }
            return returnval;
        }

        /// <summary>
        /// Gets the current title.
        /// </summary>
        /// <param name="gameId">The gameId.</param>
        /// <returns>The game's title.</returns>
        [Route("{gameId}/title")]
        [HttpGet]
        public dynamic getTitle(string gameId)
        {
            try
            {
                return new { success = true, title = Database.GetTitle(gameId) };
            }
            catch (Exception e)
            {
                return new { success = false, message = e.Message };
            }
        }

        /// <summary>
        /// Updates the game's title.
        /// </summary>
        /// <param name="gameId">The gameId.</param>
        /// <param name="value">New title.</param>
        /// <returns>Success.</returns>
        [Route("{gameId}/title")]
        [HttpPut]
        public dynamic updateTitle(string gameId, [FromBody]TitleDTO value)
        {
            try
            {
                Database.UpdateTitle(gameId, value.title);
                return new { success = true };
            }
            catch (Exception e)
            {
                return new { success = false, message = e.Message };
            }
        }

        /// <summary>
        /// Gets the current description
        /// </summary>
        /// <param name="gameId">The gameId.</param>
        /// <returns>The game's description.</returns>
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

        /// <summary>
        /// Updates the game's description.
        /// </summary>
        /// <param name="gameId">The gameId.</param>
        /// <param name="value">New title and/or description.</param>
        /// <returns>Success.</returns>
        [Route("{gameId}/description")]
        [HttpPut]
        public dynamic updateDescription(string gameId, [FromBody]DescriptionDTO value)
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

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <returns></returns>
        [Route("{gameId}/user")]
        [HttpGet]
        public dynamic getUsers(string gameId)
        {
            try
            {
                var users = Database.GetUsers(gameId).ToArray();
                if (!hasEveryoneVoted(users))
                {
                    foreach (var user in users)
                        user.Vote = null;
                }
                return new { success = true, users = convertUser(users) };
            }
            catch (Exception e)
            {
                return new { success = false, message = e.Message };
            }
        }

        private static bool hasEveryoneVoted(IEnumerable<User> users)
        {
            foreach (var user in users)
                if (user.Voted == false)
                    return false;
            return true;
        }

        /// <summary>
        /// Kicks a user from the game.
        /// (Only host can kick users.)
        /// </summary>
        /// <param name="gameId">The gameId.</param>
        /// <param name="username">The userId of the kickee.</param>
        /// <param name="value">The userId of the kicker.</param>
        /// <returns>Success.</returns>
        [Route("{gameId}/user/{username}")]
        [HttpDelete]
        public dynamic kickUser(string gameId, string username, [FromBody]UserIdDTO value)
        {
            try
            {
                Database.DeleteUser(gameId, username, value.userId);
                return new { success = true };
            }
            catch (Exception e)
            {
                return new { success = false, message = e.Message };
            }
        }

        /// <summary>
        /// Clears the votes.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [Route("{gameId}/user")]
        [HttpPut]
        public dynamic clearVotes(string gameId, [FromBody]UserIdDTO value)
        {
            try
            {
                Database.ClearVotes(gameId, value.userId);
                return new { success = true };
            }
            catch (Exception e)
            {
                return new { success = false, message = e.Message };
            }
        }

        /// <summary>
        /// User casts vote to the game.
        /// </summary>
        /// <param name="gameId">The gameId.</param>
        /// <param name="username">The userId.</param>
        /// <param name="value">The vote.</param>
        /// <returns>Success.</returns>
        [Route("{gameId}/user/{username}")]
        [HttpPut]
        public dynamic changeVote(string gameId, string username, [FromBody]VoteUserIdDTO value)
        {
            try
            {
                Database.Vote(gameId, value.userId, value.vote, username);
                return new { success = true };
            }
            catch (Exception e)
            {
                return new { success = false, message = e.Message };
            }
        }

        private IEnumerable<PublicUser> convertUser(IEnumerable<User> users)
        {
            foreach (var user in users)
                yield return new PublicUser(user.Name, user.Vote, user.Voted);
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