using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PCL2.Neo.Models.Minecraft
{
    public class Java
    {
        private static List<JavaEntity> Windows()
        {
            var javaList = new List<JavaEntity>();

            // find by environment path
            // JAVA_HOME
            var javaPath = Environment.GetEnvironmentVariable("JAVA_HOME");
            if (javaPath == null || !Directory.Exists(javaPath)) return javaList;

            var exePath = Path.Combine(javaPath, "bin\\");
            if (File.Exists(exePath))
            {
                javaList.Add(new JavaEntity(exePath));
            }

            // PATH
            javaPath = Environment.GetEnvironmentVariable("PATH");
            var pathItemList = javaPath.Split(';');
            javaPath = pathItemList.FirstOrDefault(x => x.Contains("jdk"));

            return javaList;
        }

        private static string[] Unix()
        {
            // TODO: Add code
            var javaList = new List<string>();


            return javaList.ToArray();
        }

        public static List<JavaEntity> SearchJava()
        {
            var java = new List<JavaEntity>();

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    java.AddRange(Windows());
                    break;
                case PlatformID.Unix:

                    break;
            }

            return java;
        }
    }
}
