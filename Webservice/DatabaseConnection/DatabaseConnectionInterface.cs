using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webservice.Models
{
    public interface DatabaseConnectionInterface
    {
        void CreateGame(string gameId, string userId, string username);

        void AddUser(Game game, string userId, string username);

        void UpdateTitle(Game game, string title);

        void UpdateDescription(Game game, string description);

        void Vote(Game game, string userId, string vote);

        void ClearVotes(Game game, string userId);

        void KickUser(Game game, string username);

        bool GameExists(string gameId);

        /// <summary>
        /// Gets the game with the specified gameId
        /// </summary>
        /// <param name="gameId">The game identifier.</param>
        /// <returns>The Game with the specified gameId</returns>
        /// <exception cref="KeyNotFoundException">If game could not be found</exception>
        Game GetGame(string gameId);
    }
}
