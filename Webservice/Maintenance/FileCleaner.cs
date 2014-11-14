using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Webservice.Maintenance
{
    public static class FileCleaner
    {
        private static string PATH = HttpContext.Current.Server.MapPath("~/App_Data");

        /// <summary>
        /// Deletes the files older than the given hours.
        /// </summary>
        /// <param name="hours">The hours for how long to keep a file since it was last modified before it gets deleted.</param>
        public static void DeleteFiles(int hours)
        {
            var files = Directory.GetFiles(PATH, "*.txt");
            foreach (var f in files)
            {
                if (File.GetLastWriteTime(Path.Combine(PATH, f)) < DateTime.Now.AddHours(-hours))
                    File.Delete(Path.Combine(PATH, f));
            }
        }
    }
}