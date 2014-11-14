using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using Webservice.DTOs;
using Webservice.Maintenance;
using Webservice.Models;
using Webservice.Response;

namespace Webservice.Controllers
{
    /// <summary>
    /// A Planning Poker game.
    /// </summary>
    [RoutePrefix("game")]
    public class GameController : ApiController
    {
        private static Dictionary<string, Game> games = new Dictionary<string, Game>();
        DatabaseConnectionInterface Database;

        public GameController()
        {
            Database = new FileStorage();
        }
        /// <summary>
        /// Create a new game, with the provided username as host.
        /// </summary>
        /// <param name="value">The host name.</param>
        /// <returns>The gameId and the host's userId.</returns>
        [Route("")]
        [HttpPost]
        [ResponseType(typeof(PostGameResponse))]
        public dynamic createGame([FromBody]NameDTO value)
        {
            FileCleaner.DeleteFiles(24); //Delete files which were not modified in the past 24 hours.

            string gameId = getMD5(DateTime.Now.Ticks.ToString());
            while (Database.GameExists(gameId))
                gameId = getMD5(DateTime.Now.Ticks.ToString());

            string username = value.name;
            string userId = getMD5(username + DateTime.Now.ToString());

            try
            {
                Database.CreateGame(gameId, userId, username);
                return Request.CreateResponse(HttpStatusCode.Created,
                    new PostGameResponse() { gameId = gameId, userId = userId });
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Gets all game information
        /// </summary>
        /// <param name="gameId">The gameId.</param>
        /// <returns>The game's title, description, and votes.</returns>
        [Route("{gameId}")]
        [HttpGet]
        [ResponseType(typeof(GetGameResponse))]
        public HttpResponseMessage getGame(string gameId)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                    new GetGameResponse() { username = Database.GetHost(gameId) });
            }
            catch (ArgumentException e)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
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
        [ResponseType(typeof(UserPostResponse))]
        public dynamic joinGame(string gameId, [FromBody]NameDTO value)
        {
            string username = value.name;
            string userId = getMD5(value.name);
            try
            {
                Database.AddUser(gameId, userId, username);
                return Request.CreateResponse(HttpStatusCode.Created,
                    new UserPostResponse() { userId = userId });
            }
            catch (Exception e)
            {
                return new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Gets the current title.
        /// </summary>
        /// <param name="gameId">The gameId.</param>
        /// <returns>The game's title.</returns>
        [Route("{gameId}/title")]
        [HttpGet]
        [ResponseType(typeof(GetTitleResponse))]
        public HttpResponseMessage getTitle(string gameId)
        {
            try
            {
                string title = Database.GetTitle(gameId);
                return Request.CreateResponse(HttpStatusCode.OK,
                    new GetTitleResponse() { title = title });
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
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
        public HttpResponseMessage updateTitle(string gameId, [FromBody]TitleDTO value)
        {
            string title = value.title;
            try
            {
                Database.UpdateTitle(gameId, title);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Gets the current description
        /// </summary>
        /// <param name="gameId">The gameId.</param>
        /// <returns>The game's description.</returns>
        [Route("{gameId}/description")]
        [HttpGet]
        [ResponseType(typeof(GetDescriptionResponse))]
        public HttpResponseMessage getDescription(string gameId)
        {
            try
            {
                string description = Database.GetDescription(gameId);
                return Request.CreateResponse(HttpStatusCode.OK,
                    new GetDescriptionResponse() { description = description });
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
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
        public HttpResponseMessage updateDescription(string gameId, [FromBody]DescriptionDTO value)
        {
            string description = value.description;
            try
            {
                Database.UpdateDescription(gameId, description);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <returns></returns>
        [Route("{gameId}/user")]
        [HttpGet]
        [ResponseType(typeof(List<GetUserResponse>))]
        public HttpResponseMessage getUsers(string gameId)
        {
            try
            {
                var users = Database.GetUsers(gameId).ToArray();
                if (!hasEveryoneVoted(users))
                {
                    foreach (var user in users)
                        user.Vote = null;
                }
                return Request.CreateResponse(HttpStatusCode.OK,
                    new List<GetUserResponse>(users.Select(x => new GetUserResponse() { username = x.Name, voted = x.Voted, vote = x.Vote })));
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
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
        public HttpResponseMessage kickUser(string gameId, string username, [FromBody]UserIdDTO value)
        {
            string userId = value.userId;
            try
            {
                Database.DeleteUser(gameId, username, userId);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
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
        public HttpResponseMessage clearVotes(string gameId, [FromBody]UserIdDTO value)
        {
            string userId = value.userId;
            try
            {
                Database.ClearVotes(gameId, userId);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
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
        public HttpResponseMessage changeVote(string gameId, string username, [FromBody]VoteUserIdDTO value)
        {
            string vote = value.vote;
            string userId = value.userId;
            try
            {
                Database.Vote(gameId, userId, vote, username);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
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