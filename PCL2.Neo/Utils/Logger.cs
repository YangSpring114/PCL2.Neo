using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static PCL2.Neo.Const;

namespace PCL2.Neo.Utils;

public class Logger
{
    private const int FlushInterval = 50;

    public enum LogLevel
    {
        /// <summary>
        /// 不提示，只记录日志。
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 只提示开发者。
        /// </summary>
        Developer = 1,

        /// <summary>
        /// 只提示开发者与调试模式用户。
        /// </summary>
        Debug = 2,

        /// <summary>
        /// 弹出提示所有用户。
        /// </summary>
        Hint = 3,

        /// <summary>
        /// 弹窗，不要求反馈。
        /// </summary>
        Msgbox = 4,

        /// <summary>
        /// 弹窗，要求反馈。
        /// </summary>
        Feedback = 5,

        /// <summary>
        /// 弹窗，结束程序。
        /// </summary>
        Assert = 6
    }

    public delegate void LogDelegate(string message);

    private static Logger? _instance;

    private LogDelegate _hintLogDelegate = _ => { };
    private LogDelegate _feedbackLogDelegate = _ => { };
    private LogDelegate _developerLogDelegate = _ => { };
    private LogDelegate _assertLogDelegate = _ => { };
    private LogDelegate _msgboxLogDelegate = _ => { };
    private LogDelegate _debugLogDelegate = _ => { };
    private bool _isInitSuccess = true;
    private StringBuilder _logList = new();

    public static void InitLogger(string logFilePath)
    {
        _instance = new Logger(logFilePath);
    }

    public static Logger GetInstance()
    {
        return _instance!;
    }

    private Logger(string logFilePath)
    {
        try
        {
            File.Create($"{logFilePath}Log1.txt").Dispose();
        }
        catch (IOException ex)
        {
            _isInitSuccess = false;
            Log(ex, "日志初始化失败（疑似文件占用问题）");
        }
        catch (Exception ex)
        {
            _isInitSuccess = false;
            Log(ex, "日志初始化失败", LogLevel.Developer);
        }

        try
        {
            var logWriter = new StreamWriter($"{logFilePath}Log1.txt");
            Trace.Listeners.Add(new TextWriterTraceListener(logWriter));
        }
        catch (Exception ex)
        {
            Log(ex, "日志写入失败", LogLevel.Hint);
        }

        Trace.AutoFlush = true;
        Task.Run(() =>
        {
            while (true)
            {
                if (_isInitSuccess) Flush();
                else _logList.Clear();
                Thread.Sleep(FlushInterval);
            }
        });
    }

    public void SetDelegate(LogLevel level, LogDelegate logDelegate)
    {
        switch (level)
        {
            case LogLevel.Developer: _developerLogDelegate = logDelegate; break;
            case LogLevel.Debug: _debugLogDelegate = logDelegate; break;
            case LogLevel.Hint: _hintLogDelegate = logDelegate; break;
            case LogLevel.Msgbox: _msgboxLogDelegate = logDelegate; break;
            case LogLevel.Assert: _assertLogDelegate = logDelegate; break;
            case LogLevel.Feedback: _feedbackLogDelegate = logDelegate; break;
        }
    }

    public void Log(string text, LogLevel level = LogLevel.Normal, string title = "出现错误")
    {
        string logText = $"[{TimeDateUtils.GetTimeNow()}] {text}{CrLf}";
        _logList.Append(logText);
        string msg = StringUtils.RegexReplace(text, "", @"\[[^\]]+?\] ");
        switch (level)
        {
#if DEBUG
            case LogLevel.Developer: _developerLogDelegate.Invoke(msg); break;
            case LogLevel.Debug: _debugLogDelegate.Invoke(msg); break;
#else
            case LogLevel.Developer: break;
            case LogLevel.Debug: debugLogDelegate.Invoke(msg); break; // TODO modedebug
#endif
            case LogLevel.Hint: _hintLogDelegate.Invoke(msg); break;
            case LogLevel.Msgbox: _msgboxLogDelegate.Invoke(msg); break;
            case LogLevel.Feedback: _feedbackLogDelegate.Invoke(msg); break;
            case LogLevel.Assert: _assertLogDelegate.Invoke(msg); break;
        }
    }

    public void Log(Exception ex, string desc, LogLevel level = LogLevel.Debug, string title = "出现错误")
    {
        if (ex is ThreadInterruptedException) return;
        // TODO Exception log
    }

    private readonly Lock _logFlushLock = new();

    private void Flush()
    {
        string log = "";
        lock (_logFlushLock)
        {
            if (_logList.Length > 0)
            {
                StringBuilder logListCache = _logList;
                _logList = new StringBuilder();
                log = logListCache.ToString();
            }
        }

        Trace.Write(log);
    }
}