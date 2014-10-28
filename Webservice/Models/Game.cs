using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webservice.Models
{
    public class Game
    {
        private int hostId;
        private int id;

        public int HostId { get { return hostId; } }

        public int Id { get { return id; } }

        public Game(String hostName)
        {
            this.hostId = hostName.GetHashCode();
            this.id = DateTime.Now.GetHashCode();
        }
    }
}