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

        public void AddUser(Game game, string userId, string username)
        {
            game.Users.Add(new User(userId, username));
        }

        public void UpdateTitle(Game game, string title)
        {
            game.Title = title;
        }

        public void UpdateDescription(Game game, string description)
        {
            game.Description = description;
        }
        public void Vote(Game game, string userId, string vote)
        {
            game.Users.Vote(userId, vote);
        }

        public void ClearVotes(Game game, string userId)
        {
            game.Users.ClearVotes();
        }

        public void KickUser(Game game, string username)
        {
            game.Users.Kick(username);
        }

        public bool GameExists(string gameId)
        {
            return games.ContainsKey(gameId);
        }

        public Game GetGame(string gameId)
        {
            if (games.ContainsKey(gameId))
                return games[gameId];
            else
                 throw new KeyNotFoundException("Game not found");
        }
    }
}
