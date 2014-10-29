﻿using System;
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
        private static string PATH = HttpContext.Current.Server.MapPath("~/App_Data/");

        private static string defaultVote = null;

        public static void CreateGame(string gameId, string hostId)
        {
            StreamWriter swGames = new StreamWriter(PATH + gameId + ".txt");
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
            save(PATH, lines);
        }        

        public static void UpdateDescription(string gameId, string description)
        {
            string filePath = getFile(gameId);
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
            string filePath = getFile(gameId);
            string[] lines = File.ReadAllLines(filePath);
            return lines[DESCRIPTION];
        }

        public static void Vote(string gameId, string userId, string vote)
        {
            string filePath = getFile(gameId);
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

        public static void ClearVote(string gameId, string userId, string vote)
        {
            Vote(gameId, userId, null);
        }

        public static Dictionary<string, string> GetCurrentVotes(string gameId)
        {
            Dictionary<string, string> votes = new Dictionary<string, string>();
            string filePath = getFile(gameId);
            string[] lines = File.ReadAllLines(filePath);
            foreach (string entry in lines[USER_INFO].Split(' '))
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
            foreach (var fileName in Directory.GetFiles(PATH, "*.txt").Select(Path.GetFileName))
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