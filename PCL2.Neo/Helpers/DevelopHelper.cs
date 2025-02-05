using System;
using PCL2.Neo.Utils;

namespace PCL2.Neo.Helpers;

public class DevelopHelper 
{
    /// <summary>
    /// 在开始前运行测试。
    /// </summary>
    public static void RunTest() {
        Logger.InitLogger(Const.Path);
        var L = Logger.GetInstance();
        L.SetDelegate(Logger.LogLevel.Feedback, (s) => {
            Console.WriteLine($"FEEDBACK:{s}");
        });
        L.Log("Hello");
        L.Log("FeedBack",Logger.LogLevel.Feedback);
        Logger.Stop();
    }
}