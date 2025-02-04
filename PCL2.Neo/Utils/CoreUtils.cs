using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PCL2.Neo.Utils;

/// <summary>
/// 提供核心实用工具方法的静态类。
/// </summary>
public static class CoreUtils
{
    /// <summary>
    /// 用于生成伪UUID的私有静态变量。每次调用GetUuid方法时该值递增。
    /// </summary>
    private static int _uuid = 1;

    /// <summary>
    /// 获取一个在全程序内不会重复的数字（伪 Uuid）。
    /// </summary>
    /// <returns>返回一个独一无二的整数。</returns>
    public static int GetUuid()
    {
        // 使用Interlocked.Increment确保在多线程环境下的原子性操作，避免重复。
        return Interlocked.Increment(ref _uuid);
    }

    /// <summary>
    /// 指示接取到这个异常的函数进行重试。
    /// </summary>
    public class RestartException : Exception;

    /// <summary>
    /// 指示用户手动取消了操作，或用户已知晓操作被取消的原因。
    /// </summary>
    public class CancelledException : Exception;
    
    public enum LoadState
    {
        Waiting,
        Loading,
        Finished,
        Failed,
        Aborted,
    }

    public enum Result
    {
        /// <summary>
        /// 执行成功，或进程被中断。
        /// </summary>
        Aborted = -1,

        /// <summary>
        /// 执行成功。
        /// </summary>
        Success = 0,

        /// <summary>
        /// 执行失败。
        /// </summary>
        Fail,

        /// <summary>
        /// 执行时出现未经处理的异常。
        /// </summary>
        Exception,

        /// <summary>
        /// 执行超时。
        /// </summary>
        Timeout,

        /// <summary>
        /// 取消执行。可能是由于不满足执行的前置条件。
        /// </summary>
        Cancel
    }
}