using System;
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
            User host = new User(userId, username);
            Game game = new Game(gameId, host);
            game.Users.Add(host);
            games.Add(gameId, game);
        }

        public void AddUser(string gameId, string userId, string username)
        {
            getGame(gameId).Users.Add(new User(userId, username));
        }

        public void UpdateTitle(string gameId, string title)
        {
            getGame(gameId).Title = title;
        }

        public void UpdateDescription(string gameId, string description)
        {
            getGame(gameId).Description = description;
        }

        public string GetTitle(string gameId)
        {
            return getGame(gameId).Title;
        }

        public string GetDescription(string gameId)
        {
            return getGame(gameId).Description;
        }

        public void Vote(string gameId, string userId, string vote, string username)
        {
            Game game = getGame(gameId);
            if (game.Users.Contains(userId))
                game.Users.Vote(userId, vote);
        }

        public void ClearVotes(string gameId, string userId)
        {
            Game game = getGame(gameId);
            if (game.IsHost(userId))
                game.Users.ClearVotes();
        }

        public void KickUser(string gameId, string username, string userId)
        {
            Game game = getGame(gameId);
            if (game.IsHost(userId))
                game.Users.Kick(username);
        }

        public IEnumerable<User> GetUsers(string gameId)
        {
            foreach (var user in getGame(gameId).Users)
            {
                yield return user;
            }
        }

        public bool GameExists(string gameId)
        {
            return games.ContainsKey(gameId);
        }

        public string GetHost(string gameId)
        {
            return getGame(gameId).Host.Name;
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
