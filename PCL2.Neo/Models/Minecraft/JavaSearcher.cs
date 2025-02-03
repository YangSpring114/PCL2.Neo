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

    private static readonly string[] KeySubFolderWrods =
    [
        "java", "jdk", "env", "环境", "run", "软件", "jre", "mc", "dragon",
        "soft", "cache", "temp", "corretto", "roaming", "users", "craft", "program", "世界", "net",
        "游戏", "oracle", "game", "file", "data", "jvm", "服务", "server", "客户", "client", "整合",
        "应用", "运行", "前置", "mojang", "官启", "新建文件夹", "eclipse", "microsoft", "hotspot",
        "runtime", "x86", "x64", "forge", "原版", "optifine", "官方", "启动", "hmcl", "mod", "高清",
        "download", "launch", "程序", "path", "version", "baka", "pcl", "zulu", "local", "packages",
        "4297127d64ec6", "国服", "网易", "ext", "netease", "1.", "启动"
    ];

    private const int MaxDeep = 6;

    private static List<JavaEntity> SearchFolders(string folderPath, int deep)
    {
        var entities = new List<JavaEntity>();

        if (deep >= MaxDeep)
        {
            return entities;
        }

        try
        {
            if (File.Exists(Path.Combine(folderPath, "javaw.exe"))) entities.Add(new JavaEntity(folderPath));

            var subFolder = Directory.GetDirectories(folderPath);
            var selectFolder = subFolder.Where(f => KeySubFolderWrods.Any(w => f.ToLower().Contains(w.ToLower())));
            //entities.AddRange(selectFolder.Select(SearchFolders).SelectMany(i => i).ToList());
            foreach (var folder in selectFolder)
            {
                entities.AddRange(SearchFolders(folder, deep + 1));
            }
        }
        catch (UnauthorizedAccessException)
        {
        }

        return entities;
    }

    private static List<JavaEntity> DriveJavaEntities()
    {
        var javaList = new ConcurrentBag<JavaEntity>();

        var readyDrive = DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType != DriveType.Network);
        var readyRootFolders = readyDrive.Select(d => d.RootDirectory)
            .Where(f => !f.Attributes.HasFlag(FileAttributes.ReparsePoint));

        Parallel.ForEach(readyRootFolders, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
            root =>
            {
                var entities = SearchFolders(root.ToString(), 0);
                foreach (var javaEntity in entities)
                {
                    javaList.Add(javaEntity);
                }
            });

        return javaList.ToList();
    }

    public static List<JavaEntity> SearchJava()
    {
        var javaEntities = new List<JavaEntity>();
        javaEntities.AddRange(EnvionmentJavaEntities());
        javaEntities.AddRange(DriveJavaEntities());
        return javaEntities;
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