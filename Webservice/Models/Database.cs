using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Webservice.Models
{
    public static class Database
    {
        public static void CreateGame(string gameId, string hostId)
        {
            StreamWriter swGames = File.AppendText(HttpContext.Current.Server.MapPath("~/App_Data/games.txt"));
            swGames.WriteLine(gameId + " " + hostId);
            swGames.Close();
        }

        public static void AddUser(string userId)
        {
            
        }
    }
}