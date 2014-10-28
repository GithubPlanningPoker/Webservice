using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Webservice.Models
{
    public static class Database
    {
        private static string gamesPath = HttpContext.Current.Server.MapPath("~/App_Data/games.txt");

        public static void CreateGame(string gameId, string hostId)
        {
            StreamWriter swGames = File.AppendText(gamesPath);
            swGames.WriteLine(gameId + " " + hostId);
            swGames.Close();
        }

        public static void AddUser(string gameId, string userId)
        {            
            StreamReader srGames = new StreamReader(gamesPath);
            string[] games = File.ReadAllLines(gamesPath);
            List<string> lines = new List<string>();
            string line;
            while ((line = srGames.ReadLine()) != null)
            {
                if (srGames.ReadLine().Split(' ')[0] == gameId)
                    line += " " + userId;
                lines.Add(line);
            }
            Save(lines);
        }

        private static void Save(List<string> lines)
        {
            using (StreamWriter writer = new StreamWriter(gamesPath, false))
            {
                foreach (String l in lines)
                    writer.WriteLine(l);
            }
        }

        private static IEnumerable<string> getGameIds()
        {
            StreamReader srGames = new StreamReader(gamesPath);
            string line;
            while ((line = srGames.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
}