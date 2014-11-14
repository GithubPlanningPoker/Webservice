using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Webservice.Models
{
    class Game
    {
        public string Host { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<User> Users { get; set; }
    }
}
