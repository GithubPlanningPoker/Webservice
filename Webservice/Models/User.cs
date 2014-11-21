using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webservice.Models
{
    public class User :IEquatable<User>
    {
        public User(string userId, string name, string vote = "", bool voted = false)
        {
            this.userId = userId;
            this.name = name;
            this.vote = vote;
            this.voted = voted;
        }

        private string name;

        public string Name
        {
            get { return name; }
        }

        private string userId;

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        

        private string vote;

        public string Vote
        {
            get { return vote; }
            set { vote = value; }
        }

        private bool voted;

        public bool Voted
        {
            get { return voted; }
            set { voted = value; }
        }

        public bool Equals(User other)
        {
            return this.name.Equals(other.name);
        }
    }
}