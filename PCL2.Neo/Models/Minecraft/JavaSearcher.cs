using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PCL2.Neo.Models.Minecraft.JavaSearcher;

internal class Windows
{
    private static Task<JavaExist> PathEnvSearchAsync(string path) => Task.Run(() => new JavaExist
        { IsExist = File.Exists(Path.Combine(path, "javaw.exe")), Path = path });

    private static async Task<List<JavaEntity>> EnvionmentJavaEntities()
    {
        var javaList = new List<JavaEntity>();

        // find by environment path
        // JAVA_HOME
        var javaHomePath = Environment.GetEnvironmentVariable("JAVA_HOME");
        if (javaHomePath != null || Directory.Exists(javaHomePath)) // if not exist then return
        {
            var javaPath = javaHomePath.EndsWith("bin\\")
                ? Path.Combine(javaHomePath, "javaw.exe") // check java exe file is exist
                : Path.Combine(javaHomePath, "bin", "javaw.exe");
            if (File.Exists(javaHomePath))
                javaList.Add(new JavaEntity(javaHomePath));
        }

        // PATH multi-thread
        var pathList = new ConcurrentBag<JavaExist>();
        foreach (var item in Environment.GetEnvironmentVariable("Path")!.Split(';'))
            pathList.Add(await PathEnvSearchAsync(item));

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

    public const int MaxDeep = 6;

    private static List<JavaEntity> SearchFolders(string folderPath, int deep)
    {
        var entities = new List<JavaEntity>();

        // if too deep then return
        if (deep >= MaxDeep) return entities;

        try
        {
            if (File.Exists(Path.Combine(folderPath, "javaw.exe"))) entities.Add(new JavaEntity(folderPath));

            var subFolder = Directory.GetDirectories(folderPath);
            var selectFolder = subFolder.Where(f => KeySubFolderWrods.Any(w => f.ToLower().Contains(w.ToLower())));
            //entities.AddRange(selectFolder.Select(SearchFolders).SelectMany(i => i).ToList());
            foreach (var folder in selectFolder)
                entities.AddRange(SearchFolders(folder, deep + 1)); // search sub folders
        }
        catch (UnauthorizedAccessException)
        {
            // ignore can not access folder
        }

        return entities;
    }

    private static Task<List<JavaEntity>> SearchFoldersAsync(string folderPath) => Task.Run(() =>
        SearchFolders(folderPath, 0));

    private static async Task<List<JavaEntity>> DriveJavaEntities()
    {
        var javaList = new ConcurrentBag<JavaEntity>();

        var readyDrive = DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType != DriveType.Network);
        var readyRootFolders = readyDrive.Select(d => d.RootDirectory)
            .Where(f => !f.Attributes.HasFlag(FileAttributes.ReparsePoint));

        // search java start at root folders
        // multi-thread
        foreach (var item in readyRootFolders)
        {
            var entities = await SearchFoldersAsync(item.ToString());
            foreach (var entity in entities) javaList.Add(entity);
        }

        return javaList.ToList();
    }

    public static async Task<List<JavaEntity>> SearchJava()
    {
        var javaEntities = new List<JavaEntity>();
        javaEntities.AddRange(await EnvionmentJavaEntities());
        javaEntities.AddRange(await DriveJavaEntities());
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