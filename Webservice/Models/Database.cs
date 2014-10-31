using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Webservice.Models
{
    public static class Database
    {
        private static int DESCRIPTION = 1;
        private static int USER_INFO = 2;
        private static string PATH = HttpContext.Current.Server.MapPath("~/App_Data");
        private static string defaultVote = "null";

        public static void CreateGame(string gameId, string hostId)
        {
            StreamWriter swGames = new StreamWriter(Path.Combine(PATH, gameId + ".txt"), true);
            swGames.WriteLine(hostId);
            swGames.WriteLine();
            swGames.WriteLine();
            swGames.Close();
        }

        public static void AddUser(string gameId, string userId)
        {
            string filePath = getFilePath(gameId);
            string[] lines = File.ReadAllLines(filePath);
            lines[2] += userId + "," + defaultVote;
            save(PATH, lines);
        }        

        public static void UpdateDescription(string gameId, string description)
        {
            string filePath = getFilePath(gameId);
            string[] lines = File.ReadAllLines(filePath);
            lines[DESCRIPTION] = description;
            save(filePath, lines);
        }

        public static void ClearDescription(string gameId)
        {
            UpdateDescription(gameId, "");
        }

        public static string GetDescription(string gameId)
        {
            string filePath = getFilePath(gameId);
            string[] lines = File.ReadAllLines(filePath);
            return lines[DESCRIPTION];
        }

        public static void Vote(string gameId, string userId, string vote)
        {
            if (!validVote(vote))
                throw new ArgumentException("Vote is expected to be one of the following values: 0, half, 1, 2, 3, 5, 8, 13, 20, 40, 100, inf, ? ." + " Parameter given: " + vote);
            string filePath = getFilePath(gameId);
            string[] lines = File.ReadAllLines(filePath);
            foreach (string entry in lines[USER_INFO].Split(' '))
            {
                if (userId == entry.Split(',')[0])
                {
                    entry.Split(',')[1] = vote;
                }
            }
            save(filePath, lines);
        }

        public static void ClearVote(string gameId, string userId)
        {
            Vote(gameId, userId, null);
        }

        public static Dictionary<string, string> GetCurrentVotes(string gameId)
        {
            Dictionary<string, string> votes = new Dictionary<string, string>();
            string filePath = getFilePath(gameId);
            string[] lines = File.ReadAllLines(filePath);
            foreach (string entry in lines[USER_INFO].Split(' '))
            {
                string user = entry.Split(',')[0];
                string vote = entry.Split(',')[1];
                votes.Add(user, vote);
            }
            return votes;
        }

        private static bool validVote(string vote)
        {
            switch (vote)
            {
                case null:
                case "0":
                case "half":
                case "1":
                case "2":
                case "3":
                case "5":
                case "8":
                case "13":
                case "20":
                case "40":
                case "100":
                case "inf":
                case "?":
                    return true;
                default:
                    return false;
            }
        }

        private static void save(string filePath, string[] lines)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                foreach (String l in lines)
                    writer.WriteLine(l);
                writer.Close();
            }
        }

        private static string getFilePath(string gameId)
        {
            string filePath = Path.Combine(PATH, gameId + ".txt");
            if (File.Exists(filePath))
                return filePath;
            else
                throw new ArgumentException("Game: " + gameId + " does not exist.");
        }

        public static bool GameExists(string gameId)
        {
            return File.Exists(Path.Combine(PATH, gameId + ".txt"));
        }
    }
}