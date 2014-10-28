using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Webservice.Models;

namespace Webservice.Controllers
{
    public class GameController : ApiController
    {
        // POST api/<controller>
        public Game Post([FromBody]String value)
        {
            Game game = new Game(value);

            return game;
        }
    }
}