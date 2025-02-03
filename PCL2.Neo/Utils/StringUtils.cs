using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace PCL2.Neo.Utils;

public static class StringUtils
{
    /// <summary>
    /// 返回一个枚举对应的字符串。
    /// </summary>
    public static string GetStringFromEnum(Enum enumData)
    {
        return Enum.GetName(enumData.GetType(), enumData);
    }

    /// <summary>
    /// 高速的 IndexOf。
    /// </summary>
    public static int IndexOfF(this string str, string subStr,bool ignoreCase = false)
    {
        return str.IndexOf(subStr, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
    }
    
    /// <summary>
    /// 支持可变大小写判断的 Contains。
    /// </summary>
    public static bool ContainsF(this string str,string subStr,bool ignoreCase = false)
    {
        return str.IndexOf(subStr, (ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) >= 0;
    }
}
