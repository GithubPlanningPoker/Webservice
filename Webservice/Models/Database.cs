using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Webservice.Models
{
    public enum Entry
    {
        Host = 0,
        Description = 1,
        Users = 2,
        Votes = 3
    }

    public static class Database
    {
        private static int users = 2;
        private static string gamesPath = HttpContext.Current.Server.MapPath("~/App_Data/games.txt");
        private static string descriptionsPath = HttpContext.Current.Server.MapPath("~/App_Data/descriptions.txt");
        private static string votesPath = HttpContext.Current.Server.MapPath("~/App_Data/votes.txt");
        private static string filePath = HttpContext.Current.Server.MapPath("~/App_Data/");

        private static string empty = "empty";

        public static void CreateGame(string gameId, string hostId)
        {
            StreamWriter swGames = new StreamWriter(filePath + gameId + ".txt");
            swGames.WriteLine(hostId);
            swGames.Close();
        }

        public static void AddUser(string gameId, string userId)
        {
            string fileName = getFile(gameId);
            string path = filePath + fileName;
            if (fileName == "")
                throw new ArgumentException("Game: " + gameId + " does not exist.");
            string[] lines = File.ReadAllLines(path);
            lines[2] += userId;
            save(path, lines);
        }        

        public static void UpdateDescription(string gameId, string description)
        {
            StreamReader srDescriptions = new StreamReader(descriptionsPath);
            List<string> lines = new List<string>();
            string line;
            bool gameExists = false;
            while ((line = srDescriptions.ReadLine()) != null)
            {
                if (line.Split(' ')[0] == gameId)
                {
                    line = gameId + " " + description;
                    gameExists = true;
                }
                lines.Add(line);
            }
            if (!gameExists)
                lines.Add(gameId + " " + description);
            //Save(lines);
        }

        public static void ClearDescription(string gameId)
        {
            StreamReader srGames = new StreamReader(gamesPath);
            List<string> lines = new List<string>();
            string line;
            while ((line = srGames.ReadLine()) != null)
            {
                if (line.Split(' ')[0] == gameId)
                    line = gameId;
                lines.Add(line);
            }
            //save(lines)
        }

        public static string GetDescription(string gameId)
        {
            StreamReader srGames = new StreamReader(gamesPath);
            string line, res = "";
            while ((line = srGames.ReadLine()) != null)
            {
                if (line.Split(' ')[0] == gameId)
                    res = line;
            }
            return res;
        }

        public static Dictionary<string, string> GetCurrentVotes(string gameId)
        {
            Dictionary<string, string> votes = new Dictionary<string, string>();
            StreamReader srGames = new StreamReader(gamesPath);
            string line;
            while ((line = srGames.ReadLine()) != null)
            {
                if (line.Split(' ')[0] == gameId)
                {
                    string[] x = line.Split(' ');
                    for (int i = 1; i < x.Count(); i+=2)
                    {
                        votes[x[i]] = x[i + 1];
                    }
                }
            }
            return votes;
        }

        private static void save(string filePath, string[] lines)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                foreach (String l in lines)
                    writer.WriteLine(l);
            }
        }


        private static string getFile(string gameId)
        {
            string file = "";
            foreach (var fileName in Directory.GetFiles(filePath, "*.txt").Select(Path.GetFileName))
            {
                if (gameId == fileName)
                    file = fileName;
            }
            return file;
        }
    }
}