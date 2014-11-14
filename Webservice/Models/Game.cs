using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Webservice.Models
{
    public class Game
    {
        public Game(string gameId, User host)
        {
            this.gameId = gameId;
            this.host = host;
        }
        private string gameId;

        public string GameId
        {
            get { return gameId; }
        }

        private User host;

        public User Host
        {
            get { return host; }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private List<User> users;

        public List<User> Users
        {
            get { return users; }
            set { users = value; }
        }
        
    }
}
