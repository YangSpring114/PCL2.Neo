using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static PCL2.Neo.Const;

namespace PCL2.Neo.Utils;

public class Logger
{
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
    private StreamWriter? _logWriter;

    public static void InitLogger(string logFilePath)
    {
        if(_instance == null) _instance = new Logger(logFilePath);
    }

    public static Logger GetInstance()
    {
        if(_instance != null) return _instance;
        else throw new Exception("Logger not initialized.");
    }

    public static void Stop() {
        if(_instance == null) throw new Exception("Logger not initialized.");
        if(_instance._logWriter!=null) _instance._logWriter.Dispose();
        _instance=null;
    }

    private Logger(string logFilePath)
    {
        bool _isInitSuccess = true;
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
        if(!_isInitSuccess) return;
        try
        {
            _logWriter = new StreamWriter($"{logFilePath}Log1.txt");
        }
        catch (Exception ex)
        {
            _logWriter = null;
            Log(ex, "日志写入失败", LogLevel.Hint);
        }
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
        if(_logWriter != null) _logWriter.WriteAsync(logText);
#if DEBUG 
        Debug.Write(logText);
#endif
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
}