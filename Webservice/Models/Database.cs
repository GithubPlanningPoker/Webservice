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

        private static string USER_SEPARATOR = ";:;:;:";
        private static string VALUE_SEPARATOR = "-_-_-_-_-_";
        private static string DESCRIPTION_SEPARATOR = "*********";
        private static string NEWLINE = "^*^*";

        public static void CreateGame(string gameId, string hostId, string username)
        {
            StreamWriter swGames = new StreamWriter(Path.Combine(PATH, gameId + ".txt"), true);
            swGames.WriteLine(hostId);
            swGames.WriteLine();
            swGames.WriteLine(hostId + VALUE_SEPARATOR + username + VALUE_SEPARATOR + defaultVote);
            swGames.Close();
        }

        public static void AddUser(string gameId, string userId, string username)
        {
            string filePath = getFilePath(gameId);
            string[] lines = File.ReadAllLines(filePath);
            string[] users = lines[USER_INFO].Split(USER_SEPARATOR);
            for (int i = 0; i < users.Length; i++)
            {
                string[] values = users[i].Split(VALUE_SEPARATOR);
                if (username == values[1])
                    throw new ArgumentException("Username already exists.");
            }
            lines[USER_INFO] += USER_SEPARATOR + userId + VALUE_SEPARATOR + username + VALUE_SEPARATOR + defaultVote;
            save(filePath, lines);
        }

        public static void UpdateDescription(string gameId, string title, string description)
        {
            if (title == null)
                title = "";
            if (description == null)
                description = "";
            string filePath = getFilePath(gameId);
            string[] lines = File.ReadAllLines(filePath);
            lines[DESCRIPTION] = title.Replace("\n", NEWLINE) + DESCRIPTION_SEPARATOR + description.Replace("\n", NEWLINE);
            save(filePath, lines);
        }

        public static void ClearDescription(string gameId)
        {
            UpdateDescription(gameId, "" ,"");
        }

        public static string GetDescriptionTitle(string gameId)
        {
            string filePath = getFilePath(gameId);
            string[] lines = File.ReadAllLines(filePath);
            if (lines[DESCRIPTION] == "")
                return "";
            return lines[DESCRIPTION].Split(DESCRIPTION_SEPARATOR)[0].Replace(NEWLINE,"\n");
        }

        public static string GetDescription(string gameId)
        {
            string filePath = getFilePath(gameId);
            string[] lines = File.ReadAllLines(filePath);
            if (lines[DESCRIPTION] == "")
                return "";
            return lines[DESCRIPTION].Split(DESCRIPTION_SEPARATOR)[1].Replace(NEWLINE,"\n");
        }

        public static void Vote(string gameId, string userId, string vote)
        {
            if (!validVote(vote))
                throw new ArgumentException("Vote is expected to be one of the following values: 0, half, 1, 2, 3, 5, 8, 13, 20, 40, 100, inf, ?, break ." + " Parameter given: " + vote);
            string filePath = getFilePath(gameId);
            string[] lines = File.ReadAllLines(filePath);
            string[] users = lines[USER_INFO].Split(USER_SEPARATOR);
            for (int i = 0; i < users.Length; i++)
            {
                string[] values = users[i].Split(VALUE_SEPARATOR);
                if (userId == values[0])
                {
                    if (values[2] != defaultVote)
                        throw new ArgumentException("The user: " + values[1] + " has already voted.");
                    values[2] = vote;
                    users[i] = string.Join(VALUE_SEPARATOR, values);
                    break;
                }
            }
            lines[2] = string.Join(USER_SEPARATOR, users);
            save(filePath, lines);
        }

        public static void ClearVotes(string gameId, string userId)
        {
            string filePath = getFilePath(gameId);
            string[] lines = File.ReadAllLines(filePath);
            string[] users = lines[USER_INFO].Split(USER_SEPARATOR);
            isHost(users, userId);
            for (int i = 0; i < users.Length; i++)
            {
                string[] values = users[i].Split(VALUE_SEPARATOR);
                values[2] = defaultVote;
                users[i] = string.Join(VALUE_SEPARATOR, values);
            }
            lines[2] = string.Join(USER_SEPARATOR, users);
            save(filePath, lines);
        }

        public static void DeleteUser(string gameId, string username, string userId)
        {
            string filePath = getFilePath(gameId);
            string[] lines = File.ReadAllLines(filePath);
            string[] users = lines[USER_INFO].Split(USER_SEPARATOR);
            isHost(users, userId);
            for (int i = 0; i < users.Length; i++)
            {
                if (users[i].Split(VALUE_SEPARATOR)[1] == username)
                    users[i] = "";
            }
        }

        private static void isHost(string[] users, string userId)
        {
            if (users[0].Split(VALUE_SEPARATOR)[0] == userId)
                return;
            else
                throw new ArgumentException("The user id given must be the host to clear votes.");
        }

        public static IEnumerable<object> GetUsers(string gameId)
        {
            string filePath = getFilePath(gameId);
            string[] lines = File.ReadAllLines(filePath);
            foreach (string entry in lines[USER_INFO].Split(USER_SEPARATOR))
            {
                string[] values = entry.Split(VALUE_SEPARATOR);
                string name = values[1];
                string vote = values[2];
                if (vote == "null")
                    vote = null;
                yield return new { name, vote };
            }
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
                case "break":
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

        internal static object GetHost(string gameId)
        {
            throw new NotImplementedException();
        }

        internal static void UpdateTitle(string gameId, string p)
        {
            throw new NotImplementedException();
        }
    }

    public static class StringSplitter
    {
        public static string[] Split(this string text, string split)
        {
            return text.Split(new string[] { split }, StringSplitOptions.None);
        }
    }
}