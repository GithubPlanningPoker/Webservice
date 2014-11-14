using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webservice.Models
{
    public interface DatabaseConnectionInterface
    {
        void CreateGame(string gameId, string hostId, string username);

        void AddUser(string gameId, string userId, string username);

        void UpdateTitle(string gameId, string title);

        void UpdateDescription(string gameId, string description);

        string GetTitle(string gameId);

        string GetDescription(string gameId);

        void Vote(string gameId, string userId, string vote, string username);

        void ClearVotes(string gameId, string userId);

        void KickUser(string gameId, string username, string userId);

        IEnumerable<User> GetUsers(string gameId);

        bool GameExists(string gameId);

        string GetHost(string gameId);
    }
}
