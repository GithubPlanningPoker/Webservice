﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Webservice.Models
{
    public class MemoryDatabase : DatabaseConnectionInterface
    {
        private static Dictionary<string, Game> games = new Dictionary<string, Game>();

        public void CreateGame(string gameId, string userId, string username)
        {
            games.Add(gameId, new Game(gameId, new User(userId, username)));
        }

        public void AddUser(string gameId, string userId, string username)
        {
            
        }

        public void UpdateTitle(string gameId, string title)
        {
            throw new NotImplementedException();
        }

        public void UpdateDescription(string gameId, string description)
        {
            throw new NotImplementedException();
        }

        public string GetTitle(string gameId)
        {
            throw new NotImplementedException();
        }

        public string GetDescription(string gameId)
        {
            throw new NotImplementedException();
        }

        public void Vote(string gameId, string userId, string vote, string username)
        {
            throw new NotImplementedException();
        }

        public void ClearVotes(string gameId, string userId)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(string gameId, string username, string userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsers(string gameId)
        {
            throw new NotImplementedException();
        }

        public bool GameExists(string gameId)
        {
            throw new NotImplementedException();
        }

        public string GetHost(string gameId)
        {
            throw new NotImplementedException();
        }

        private Game getGame(string gameId)
        {
            if (games.ContainsKey(gameId))
                return games[gameId];
            else
                throw new KeyNotFoundException("The game does not exist.");
        }
    }
}
