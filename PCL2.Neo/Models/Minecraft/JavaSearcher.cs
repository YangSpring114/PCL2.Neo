using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PCL2.Neo.Models.Minecraft.JavaSearcher;

internal class Windows
{
    private static List<JavaEntity> EnvionmentJavaEntities()
    {
        var javaList = new List<JavaEntity>();

        // find by environment path
        // JAVA_HOME
        var javaHomePath = Environment.GetEnvironmentVariable("JAVA_HOME");
        if (javaHomePath != null || Directory.Exists(javaHomePath)) // if not exist then return
        {
            var javaPath = javaHomePath.EndsWith("bin\\")
                ? Path.Combine(javaHomePath, "javaw.exe")
                : Path.Combine(javaHomePath, "bin", "javaw.exe");
            if (File.Exists(javaHomePath))
                javaList.Add(new JavaEntity(javaHomePath));
        }

        // PATH
        var pathList = new ConcurrentBag<JavaExist>();
        Parallel.ForEach(Environment.GetEnvironmentVariable("Path")!.Split(';') /* get path list */,
            new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            },
            jPath =>
            {
                pathList.Add(new JavaExist
                    { IsExist = File.Exists(Path.Combine(jPath, "javaw.exe")), Path = jPath });
            });
        javaList.AddRange(pathList.Where(j => j.IsExist).Select(j => new JavaEntity(j.Path)));

        return javaList;
    }

    private static List<JavaEntity> SearchFolders(string folderPath)
    {
    }

    private static List<JavaEntity> DriveJavaEntities()
    {
        var javaList = new ConcurrentBag<JavaEntity>();

        var readyDrive = DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType != DriveType.Network);
        var readyRootFolders = readyDrive.Select(d => d.RootDirectory);

        Parallel.ForEach(readyRootFolders, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
            info =>
            {
                /* TODO: Add code */
            });

        return javaList.ToList();
    }

    private static List<JavaEntity> AppdataJavaEntities()
    {
    }

    public static List<JavaEntity> SearchJava()
    {
    }
}

internal class Unix
{
    public static List<JavaEntity> SerachJava()
    {
        var javaList = new List<JavaEntity>();

        // TODO: Add code

        return javaList;
    }
}