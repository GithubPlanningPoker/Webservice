using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web;

namespace Webservice.Models
{
    public static class Database
    {
        public void CreateGame(int gameId, int hostId)
        {
            var sw = File.AppendText(HttpContext.Current.Server.MapPath("~/App_Data/games.txt"));
            sw.WriteLine(gameId.ToString(), hostId.ToString());
        }
    }
}