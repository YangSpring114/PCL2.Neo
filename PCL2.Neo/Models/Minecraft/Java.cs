using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PCL2.Neo.Models.Minecraft
{
    internal class JavaExist
    {
        public bool IsExist { get; set; }
        public string Path { get; set; }
    }

    public class Java
    {
        private static List<JavaEntity> Windows()
        {
            var javaList = new List<JavaEntity>();

            // find by environment path
            // JAVA_HOME
            var javaHomePath = Environment.GetEnvironmentVariable("JAVA_HOME");
            if (javaHomePath == null || !Directory.Exists(javaHomePath)) return javaList;

            var binPath = Path.Combine(javaHomePath, "bin\\");
            if (File.Exists(binPath))
            {
                javaList.Add(new JavaEntity(binPath));
            }

            // PATH
            var pathItemList = Environment.GetEnvironmentVariable("PATH").Split(';');
            var pathList = new List<JavaExist>();
            Parallel.ForEach(pathItemList,
                jPath =>
                {
                    var exePath = Path.Combine(jPath, "java.exe");
                    var wexePath = Path.Combine(jPath, "javaw.exe");
                    pathList.Add(new JavaExist
                        { IsExist = File.Exists(exePath) && File.Exists(wexePath), Path = jPath });
                });

            javaList.AddRange(pathList.Where(j => j.IsExist).Select(j => new JavaEntity(j.Path)));
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
