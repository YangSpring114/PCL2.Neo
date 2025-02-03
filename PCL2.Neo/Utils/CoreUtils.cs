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
}