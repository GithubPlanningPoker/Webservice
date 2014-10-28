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
        private static string path = HttpContext.Current.Server.MapPath("~/App_Data/");

        private static string defaultVote = "NotVotedYet";

        public static void CreateGame(string gameId, string hostId)
        {
            StreamWriter swGames = new StreamWriter(path + gameId + ".txt");
            swGames.WriteLine(hostId);
            swGames.WriteLine();
            swGames.WriteLine();
            swGames.Close();
        }

        public static void AddUser(string gameId, string userId)
        {
            string filePath = getFile(gameId);            
            string[] lines = File.ReadAllLines(filePath);
            lines[2] += userId + "," + defaultVote;
            save(path, lines);
        }        

        public static void UpdateDescription(string gameId, string description)
        {
            string filePath = getFile(gameId);
            string[] lines = File.ReadAllLines(filePath);
            lines[1] = description;
            save(filePath, lines);
        }

        public static void ClearDescription(string gameId)
        {
            UpdateDescription(gameId, "");
        }

        public static string GetDescription(string gameId)
        {
            string filePath = getFile(gameId);
            string[] lines = File.ReadAllLines(filePath);
            return lines[1];
        }

        public static Dictionary<string, string> GetCurrentVotes(string gameId)
        {
            Dictionary<string, string> votes = new Dictionary<string, string>();
            string filePath = getFile(gameId);
            string[] lines = File.ReadAllLines(filePath);
            foreach (string entry in lines[2].Split(' '))
            {
                string user = entry.Split(',')[0];
                string vote = entry.Split(',')[1];
                votes.Add(user, vote);
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
            foreach (var fileName in Directory.GetFiles(path, "*.txt").Select(Path.GetFileName))
            {
                if (gameId == fileName)
                    file = fileName;
            }
            if (file == "")
                throw new ArgumentException("Game: " + gameId + " does not exist.");
            return file;
        }
    }
}