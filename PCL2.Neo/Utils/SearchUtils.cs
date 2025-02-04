using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCL2.Neo.Utils;

public static class SearchUtils
{
    /// <summary>
    /// 用于搜索的项目。
    /// </summary>
    public class SearchEntry<T>
    {
        /// <summary>
        /// 该项目对应的源数据。
        /// </summary>
        public T item;
        /// <summary>
        /// 该项目用于搜索的源。
        /// </summary>
        public List<KeyValuePair<string, double>> searchSource;
        /// <summary>
        /// 相似度。
        /// </summary>
        public double similarity;
        /// <summary>
        /// 是否完全匹配。
        /// </summary>
        public bool absoluteRight;
    }

    /// <summary>
    /// 获取搜索文本的相似度。
    /// </summary>
    /// <param name="source">被搜索的长内容。</param>
    /// <param name="query">用户输入的搜索文本。</param>
    private static double SearchSimilarity(string source, string query)
    {
        int qp = 0;
        double lenSum = 0;
        source = source.ToLower().Replace(" ", "");
        query = query.ToLower().Replace(" ", "");
        int sourceLength = source.Length;
        int queryLength = query.Length; // 用于计算最后因数的长度缓存
        while (qp < queryLength)
        {
            // 对 qp 作为开始位置计算
            int sp = 0, lenMax = 0, spMax = 0;
            // 查找以 qp 为头的最大子串
            while (sp < source.Length)
            {
                // 对每个 sp 作为开始位置计算最大子串
                int len = 0;
                while (qp + len < queryLength && (sp + len) < source.Length && source[sp + len] == query[qp + len])
                {
                    len += 1;
                }
                // 存储 len
                if (len > lenMax)
                {
                    lenMax = len;
                    spMax = sp;
                }
                // 根据结果增加 sp
                sp += Math.Max(1, len);
            }
            if (lenMax > 0)
            {
                source = source.Substring(0, spMax) + ((source.Count() > spMax + lenMax) ? source.Substring(spMax + lenMax) : string.Empty);
                // 存储 lenSum
                var incWeight = (Math.Pow(1.4, 3 + lenMax) - 3.6); // 根据长度加成
                incWeight *= (1 + 0.3 * Math.Max(0, 3 - Math.Abs(qp - spMax))); // 根据位置加成
                lenSum += incWeight;
            }
            // 根据结果增加 qp
            qp += Math.Max(1, lenMax);
        }
        return (lenSum / queryLength) * (3 / Math.Pow(sourceLength + 15, 0.5)) * (queryLength <= 2 ? 3 - queryLength : 1);
    }
    /// <summary>
    /// 获取多段文本加权后的相似度。
    /// </summary>
    private static double SearchSimilarityWeighted(List<KeyValuePair<string, double>> source, string query)
    {
        double totalWeight = 0;
        double sum = 0;
        foreach (var pair in source)
        {
            sum += SearchSimilarity(pair.Key, query) * pair.Value;
            totalWeight += pair.Value;
        }
        return sum / totalWeight;
    }

    public static List<SearchEntry<T>> Search<T>(List<SearchEntry<T>> entries, string query, int maxBlurCount = 5, double minBlurSimilarity = 0.1)
    {
        // 初始化
        List<SearchEntry<T>> resultList = new List<SearchEntry<T>>();
        if (!entries.Any()) return resultList;
        // 进行搜索，获取相似信息
        foreach (var entry in entries)
        {
            entry.similarity = SearchSimilarityWeighted(entry.searchSource, query);
            entry.absoluteRight = query.Split(" ").All((queryPart) => entry.searchSource.Any((source) => source.Key.Replace(" ","").ContainsF(queryPart, true)));
        }
        // 按照相似度进行排序
        entries.Sort((left, right) =>
        {
            if (left.absoluteRight ^ right.absoluteRight)
            {
                return left.absoluteRight;
            } else
            {
                return left.similarity > right.similarity;
            }
        });
        // 返回结果
        int blurCount = 0;
        foreach (var entry in entries)
        {
            if (entry.absoluteRight)
            {
                resultList.Add(entry);
            }
            else
            {
                if (entry.similarity < minBlurSimilarity || blurCount == maxBlurCount) break;
                resultList.Add(entry);
                blurCount++;
            }
        }
        return resultList;
    }
}
