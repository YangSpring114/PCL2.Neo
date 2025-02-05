using System;
using System.Text;

namespace PCL2.Neo.Utils;

public static class TimeDateUtils
{
    /// <summary>
    /// 获取格式类似于“11:08:52.037”的当前时间的字符串。
    /// </summary>
    public static string GetTimeNow() => DateTime.Now.ToString("HH':'mm':'ss'.'fff");

    /// <summary>
    /// 获取系统运行时间（毫秒），保证为正 Long 且大于 1，但可能突变减小。
    /// </summary>
    public static long GetTimeTick() => Environment.TickCount + 2147483651L;

    public enum UnixTimeType
    {
        Seconds,
        Milliseconds
    }

    /// <summary>
    /// 获取十进制 Unix 时间戳。
    /// </summary>
    public static long GetUnixTimestamp(UnixTimeType type)
    {
        return type switch
        {
            UnixTimeType.Seconds => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
            UnixTimeType.Milliseconds => new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    /// <summary>
    /// 时间戳转化为日期。
    /// </summary>
    public static DateTime GetDate(long timeStamp, UnixTimeType type)
    {
        return type switch
        {
            UnixTimeType.Seconds => DateTimeOffset.FromUnixTimeSeconds(timeStamp).LocalDateTime,
            UnixTimeType.Milliseconds => DateTimeOffset.FromUnixTimeMilliseconds(timeStamp).LocalDateTime,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    /// <summary>
    /// 将 UTC 时间转化为当前时区的时间。
    /// </summary>
    public static DateTime GetLocalTime(DateTime utcDate)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcDate, TimeZoneInfo.Local);
    }

    /// <summary>
    /// 将时间间隔转换为类似“5 分 10 秒前”的易于阅读的形式。
    /// </summary>
    public static string GetTimeSpanString(TimeSpan span, bool isShortForm)
    {
        string result;
        var endFix = span.TotalMilliseconds > 0 ? "后" : "前";
        if (span.TotalMilliseconds < 0) span = -span;
        var totalMonthes = Math.Floor((double)(span.Days / 30));
        if (isShortForm)
        {
            if (totalMonthes >= 12) result = Math.Floor(totalMonthes / 12) + " 年";
            else if (totalMonthes >= 2) result = totalMonthes + " 个月";
            else if (span.TotalDays >= 2) result = span.Days + " 天";
            else if (span.TotalHours >= 1) result = span.Hours + " 小时";
            else if (span.TotalMinutes >= 1) result = span.Minutes + " 分钟";
            else if (span.TotalSeconds >= 1) result = span.Seconds + " 秒";
            else result = "1 秒";
        }
        else
        {
            if (totalMonthes >= 61) result = Math.Floor(totalMonthes / 12) + " 年";
            else if (totalMonthes >= 12)
                result = Math.Floor(totalMonthes / 12) + " 年" +
                         ((totalMonthes % 12) > 0 ? " " + (totalMonthes % 12) + " 个月" : "");
            else if (totalMonthes >= 4) result = totalMonthes + " 个月";
            else if (totalMonthes >= 1)
                result = totalMonthes + " 月" + ((span.Days % 30) > 0 ? " " + (span.Days % 30) + " 天" : "");
            else if (span.TotalDays >= 4) result = span.Days + " 天";
            else if (span.TotalDays >= 1) result = span.Days + " 天" + (span.Hours > 0 ? " " + span.Hours + " 小时" : "");
            else if (span.TotalHours >= 10) result = span.Hours + " 小时";
            else if (span.TotalHours >= 1)
                result = span.Hours + " 小时" + (span.Minutes > 0 ? " " + span.Minutes + " 分钟" : "");
            else if (span.TotalMinutes >= 10) result = span.Minutes + " 分钟";
            else if (span.TotalMinutes >= 1)
                result = span.Minutes + " 分" + (span.Seconds > 0 ? " " + span.Seconds + " 秒" : "");
            else if (span.TotalSeconds >= 1) result = span.Seconds + " 秒";
            else result = "1 秒";
        }
        return result + endFix;
    }
}
