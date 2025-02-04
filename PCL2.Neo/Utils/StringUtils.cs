using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

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
    /// 将文件大小转化为适合的文本形式，如“1.28 M”。
    /// </summary>
    public static string GetFileSizeString(long fileSize)
    {
        var isNegative = fileSize < 0;
        if (isNegative) fileSize *= -1;
        if (fileSize < 1000)
        {
            return (isNegative ? "-" : "") + fileSize + " B";
        }
        if (fileSize < 1024 * 1000)
        {
            string roundResult = Math.Round((double)fileSize / 1024).ToString();
            return (isNegative ? "-" : "") + Math.Round((double)fileSize / 1024,(int)MathUtils.MathClamp(3-roundResult.Length,0,2)) + " K";
        }
        if (fileSize < 1024 * 1024 * 1000)
        {
            string roundResult = Math.Round((double)fileSize / 1024 / 1024).ToString();
            return (isNegative ? "-" : "") + Math.Round((double)fileSize  / 1024 / 1024,(int)MathUtils.MathClamp(3-roundResult.Length,0,2)) + " M";
        }
        else
        {
            string roundResult = Math.Round((double)fileSize / 1024 / 1024 / 1024).ToString();
            return (isNegative ? "-" : "") + Math.Round((double)fileSize / 1024 / 1024 / 1024,(int)MathUtils.MathClamp(3-roundResult.Length,0,2)) + " G";
        }
    }

    /// <summary>
    /// 将第一个字符转换为大写，其余字符转换为小写。
    /// </summary>
    public static string Capitalize(this string word)
    {
        if (string.IsNullOrEmpty(word)) return word;
        return word.Substring(0, 1).ToUpperInvariant() + word.Substring(1).ToLowerInvariant();
    }

    /// <summary>
    /// 将字符串统一至某个长度，过短则以 Code 将其右侧填充，过长则截取靠左的指定长度。
    /// </summary>
    public static string StrFill(string str, string code, byte length)
    {
        if(str.Length > length) return str.Substring(0, length);
        return str.PadRight(length, Convert.ToChar(code));
    }

    /// <summary>
    /// 将一个小数显示为固定的小数点后位数形式，将向零取整。
    /// 如 12 保留 2 位则输出 12.00，而 95.678 保留 2 位则输出 95.67。
    /// </summary>
    public static string StrFillNum(double num, int length)
    {
        string result = "";
        num = Math.Round(num, length,MidpointRounding.AwayFromZero);
        result = num.ToString();
        if (!result.Contains(".")) return (result+".").PadRight(result.Length+1+length, '0');
        return result.PadRight(result.Split(".")[0].Length + 1 + length, '0');
    }

    /// <summary>
    /// 移除字符串首尾的标点符号、回车，以及括号中、冒号后的补充说明内容。
    /// </summary>
    public static string StrTrim(string str,bool removeQuote = true)
    {
        if (removeQuote) str = str.Split("（")[0].Split("：")[0].Split("(")[0].Split(":")[0];
        return str.Trim('.', '。', '！', ' ', '!', '?', '？', '“', '”');
    }

    /// <summary>
    /// 连接字符串。
    /// </summary>
    public static string Join(this IEnumerable list, string split)
    {
        StringBuilder builder = new StringBuilder();
        bool isFirst = true;
        foreach (var element in list)
        {
            if(isFirst) isFirst = false;
            else builder.Append(split);
            if(element!=null) builder.Append(element);
        }
        return builder.ToString();
    }

    /// <summary>
    /// 分割字符串。
    /// </summary>
    public static string[] Split(this string fullStr, string splitStr)
    {
        return splitStr.Length == 1 ? fullStr.Split(splitStr[0]) : fullStr.Split([splitStr], StringSplitOptions.None);
    }

    /// <summary>
    /// 获取字符串哈希值。
    /// </summary>
    public static ulong GetHash(string str)
    {
        ulong result = 5381;
        for (int i = 0; i <= str.Length - 1; i++)
        {
            result = (result << 5) ^ result ^ str[i];
        }
        return result ^ 12218072394304324399UL;
    }

    /// <summary>
    /// 获取字符串 MD5。
    /// </summary>
    public static string GetStringMD5(string str)
    {
        MD5 md5Hasher = MD5.Create();
        byte[] hashedDataBytes;
        hashedDataBytes = md5Hasher.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(str));
        StringBuilder sb = new StringBuilder();
        foreach (byte i in hashedDataBytes)
        {
            sb.Append(i.ToString("x2"));
        }
        return sb.ToString();
    }

    /// <summary>
    /// 检查字符串中的字符是否均为 ASCII 字符。
    /// </summary>
    public static bool IsASCII(this string input)
    {
        return input.All((c) => c < 128);
    }

    /// <summary>
    /// 获取在子字符串第一次出现之前的部分，例如对 2024/11/08 拆切 / 会得到 2024。
    /// 如果未找到子字符串则不裁切。
    /// </summary>
    public static string BeforeFirst(this string str, string text, bool ignoreCase = false)
    {
        int pos = (String.IsNullOrEmpty(text) ? -1 : str.IndexOfF(text, ignoreCase));
        return (pos >= 0 ? str.Substring(0, pos) : str);
    }

    /// <summary>
    /// 获取在子字符串最后一次出现之前的部分，例如对 2024/11/08 拆切 / 会得到 2024/11。
    /// 如果未找到子字符串则不裁切。
    /// </summary>
    public static string BeforeLast(this string str, string text, bool ignoreCase = false)
    {
        int pos = (String.IsNullOrEmpty(text) ? -1 : str.LastIndexOfF(text, ignoreCase));
        return (pos >= 0 ? str.Substring(0, pos) : str);
    }

    /// <summary>
    /// 获取在子字符串第一次出现之后的部分，例如对 2024/11/08 拆切 / 会得到 11/08。
    /// 如果未找到子字符串则不裁切。
    /// </summary>
    public static string AfterFirst(this string str, string text, bool ignoreCase = false)
    {
        int pos = (String.IsNullOrEmpty(text) ? -1 : str.IndexOfF(text, ignoreCase));
        return (pos >= 0 ? str.Substring(pos+text.Length) : str);
    }
    
    /// <summary>
    /// 获取在子字符串最后一次出现之后的部分，例如对 2024/11/08 拆切 / 会得到 08。
    /// 如果未找到子字符串则不裁切。
    /// </summary>
    public static string AfterLast(this string str, string text, bool ignoreCase = false)
    {
        int pos = (String.IsNullOrEmpty(text) ? -1 : str.LastIndexOfF(text, ignoreCase));
        return (pos >= 0 ? str.Substring(pos+text.Length) : str);
    }

    /// <summary>
    /// 获取处于两个子字符串之间的部分，裁切尽可能多的内容。
    /// 如果未找到子字符串则不裁切。
    /// </summary>
    /// <param name="str"></param>
    /// <param name="after"></param>
    /// <param name="before"></param>
    /// <param name="ignoreCase"></param>
    /// <returns></returns>
    public static string Between(this string str, string after, string before, bool ignoreCase = false)
    {
        int startPos = String.IsNullOrEmpty(after) ? -1 : str.LastIndexOfF(after, ignoreCase);
        if (startPos >= 0) startPos += after.Length;
        else startPos = 0;
        int endPos = String.IsNullOrEmpty(before) ? -1 : str.IndexOfF(before, startPos,ignoreCase);
        if(endPos >= 0) return str.Substring(startPos, endPos - startPos);
        else if (startPos > 0) return str.Substring(startPos);
        else return str;
    }

    /// <summary>
    /// 高速的 StartsWith。
    /// </summary>
    public static bool StartsWithF(this string str, string prefix, bool ignoreCase = false)
    {
        return str.StartsWith(prefix, (ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
    }

    /// <summary>
    /// 高速的 EndsWith。
    /// </summary>
    public static bool EndsWithF(this string str, string suffix, bool ignoreCase = false)
    {
        return str.EndsWith(suffix, (ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal));
    }
    
    /// <summary>
    /// 支持可变大小写判断的 Contains。
    /// </summary>
    public static bool ContainsF(this string str,string subStr,bool ignoreCase = false)
    {
        return str.IndexOf(subStr, (ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) >= 0;
    }
    
    /// <summary>
    /// 高速的 IndexOf。
    /// </summary>
    public static int IndexOfF(this string str, string subStr,bool ignoreCase = false)
    {
        return str.IndexOf(subStr, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
    }

    /// <summary>
    /// 高速的 IndexOf。
    /// </summary>
    public static int IndexOfF(this string str, string subStr, int startIndex, bool ignoreCase = false)
    {
        return str.IndexOf(subStr, startIndex, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
    }

    /// <summary>
    /// 高速的 LastIndexOf。
    /// </summary>
    public static int LastIndexOfF(this string str, string subStr, bool ignoreCase = false)
    {
        return str.LastIndexOf(subStr, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
    }

    /// <summary>
    /// 高速的 LastIndexOf。
    /// </summary>
    public static int LastIndexOfF(this string str, string subStr, int startIndex, bool ignoreCase = false)
    {
        return str.LastIndexOf(subStr, startIndex, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
    }

    /// <summary>
    /// 不会报错的 Val。
    /// 如果输入有误，返回 0。
    /// </summary>
    public static double Val(object str)
    {
        try
        {
            return (str is string && str == "&" ? 0 : Double.Parse((string)str));
        }
        catch (Exception)
        {
            return 0;
        }
    }

    /// <summary>
    /// 为字符串进行 XML 转义。
    /// </summary>
    public static string EscapeXML(string str)
    {
        if (str.StartsWithF("{")) str = "{}" + str;
        return str.
            Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").
            Replace("\"", "&quot;").Replace("\r\n", "&#xa;");
    }

    /// <summary>
    /// 搜索字符串中的所有正则匹配项。
    /// </summary>
    public static List<string> RegexSearch(string str, string regex, RegexOptions options = RegexOptions.None)
    {
        try
        {
            List<string> result = [];
            var regexSearchRes = new Regex(regex, options).Matches(str);
            if (regexSearchRes==null) return result;
            foreach (Match item in regexSearchRes)
            {
                result.Add(item.Value);
            }
            return result;
        }
        catch (Exception e)
        {
            // TODO Log(ex, "正则匹配全部项出错");
            return [];
        }
    }

    /// <summary>
    /// 获取字符串中的第一个正则匹配项，若无匹配则返回 Nothing。
    /// </summary>
    public static string? RegexSeek(string str, string regex, RegexOptions options = RegexOptions.None)
    {
        try
        {
            var result = Regex.Match(str, regex, options).Value;
            return (result == "" ? null : result);
        }
        catch (Exception ex)
        {
            //TODO Log(ex, "正则匹配第一项出错");
            return null;
        }
    }
    
    /// <summary>
    /// 检查字符串是否匹配某正则模式。
    /// </summary>
    public static bool RegexCheck(string str, string regex, RegexOptions options = RegexOptions.None)
    {
        try
        {
            return Regex.IsMatch(str, regex, options);
        }
        catch (Exception ex)
        {
            //TODO Log(ex, "正则检查出错");
            return false;
        }
    }
    
    /// <summary>
    /// 进行正则替换，会抛出错误。
    /// </summary>
    public static string RegexReplace(string input, string replacement, string regex, RegexOptions options = RegexOptions.None)
    {
        return Regex.Replace(input, regex, replacement, options);
    }
    
    /// <summary>
    /// 对每个正则匹配分别进行替换，会抛出错误。
    /// </summary>
    public static string RegexReplaceEach(string input, MatchEvaluator replacement, string regex, RegexOptions options = RegexOptions.None)
    {
        return Regex.Replace(input, regex, replacement, options);
    }
}
