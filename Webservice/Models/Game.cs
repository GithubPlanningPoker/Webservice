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
            this.users = new UserCollection();
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

        private UserCollection users;

        public UserCollection Users
        {
            get { return users; }
            set { users = value; }
        }

        public bool IsHost(string userId)
        {
            if (host.UserId == userId)
                return true;
            throw new ArgumentException("This user: " + userId + " is not the host.");
        }

        public class UserCollection
        {
            private List<User> users;
            public UserCollection()
            {
                users = new List<User>();
            }

            public void Add(User user)
            {
                users.Add(user);
            }

            public void Remove(User user)
            {
                users.Remove(user);
            }

            public void Remove(string userId)
            {
                if (Contains(userId))
                    users.Remove(getUser(userId));
            }

            public void Kick(string username)
            {
                foreach (var user in users)
                {
                    if (user.Name == username)
                    {
                        Remove(user);
                    }
                }
                throw new ArgumentException("The user: " + username + " does not exisst.");
            }

            private User getUser(string userId)
            {
                if (Contains(userId))
                {
                    foreach (var user in users)
                    {
                        if (user.UserId == userId)
                        {
                            return user;
                        }
                    }
                }
                return null;
            }

            public void Vote(string userId, string vote)
            {
                User user = getUser(userId);
                user.Vote = vote;
                user.Voted = true;
            }

            public void ClearVotes()
            {
                foreach (var user in users)
                {
                    user.Vote = "";
                    user.Voted = false;
                }
            }

            public bool Contains(string userId)
            {
                foreach (var user in users)
                {
                    if (user.UserId == userId)
                        return true;
                }
                throw new ArgumentException("The user does not exist.");
            }

            public IEnumerator<User> GetEnumerator()
            {
                return users.GetEnumerator();
            }
        }

    }
}
