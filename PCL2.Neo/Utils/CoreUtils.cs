using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PCL2.Neo.Utils;

public static class CoreUtils
{
    private static int _uuid = 1;

    /// <summary>
    /// 获取一个全程序内不会重复的数字（伪 Uuid）。
    /// </summary>
    public static int GetUuid()
    {
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
}