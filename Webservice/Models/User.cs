using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webservice.Models
{
    public class User
    {
        public User(string name, string vote, bool voted)
        {
            this.name = name;
            this.vote = vote;
            this.voted = voted;
        }

        private string name;

        public string Name
        {
            get { return name; }
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
        }
        
        
        
    }
}