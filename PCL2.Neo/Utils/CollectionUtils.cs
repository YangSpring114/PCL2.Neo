using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCL2.Neo.Utils;

public static class CollectionUtils
{
    /// <summary>
    /// 可以使用 Equals 和等号的 List。
    /// </summary>
    public class EqualableList<T> : List<T>
    {
        public override bool Equals(object? obj)
        {
            if (!(obj is List<T>)) return false; // 类型不同
            List<T> objList = (List<T>)obj; // 类型相同
            if (objList.Count != Count) return false;
            for (int i = 0; i < objList.Count-1; i++)
            {
                if (!objList[i].Equals(this[i])) return false;
            }
            return true;
        }

        public static bool operator ==(EqualableList<T> left, EqualableList<T> right)
        {
            return EqualityComparer<EqualableList<T>>.Default.Equals(left, right);
        }
        public static bool operator !=(EqualableList<T> left, EqualableList<T> right)
        {
            return !(left == right);
        }
    }
    
    /// <summary>
    /// 线程安全的，可以直接使用 For Each 的 List。
    /// 在使用 For Each 循环时，列表的结果可能并非最新，但不会抛出异常。
    /// </summary>
    public class SafeList<T> : SynchronizedCollection<T>, IEnumerable, IEnumerable<T>
    {
        public SafeList() : base() { }
        public SafeList(IEnumerable<T> data) : base(new object(), data) { }
        public static implicit operator SafeList<T>(List<T> data)
        {
            return new SafeList<T>(data);
        }
        public static implicit operator List<T>(SafeList<T> data)
        {
            return new List<T>(data);
        }
        // 基于 SyncLock 覆写
        public new IEnumerator<T> GetEnumerator()
        {
            lock (SyncRoot) return Items.ToList().GetEnumerator();
        }
        private IEnumerator GetEnumeratorGeneral()
        {
            lock (SyncRoot) return Items.ToList().GetEnumerator();  
        }
    }

    /// <summary>
    /// 按照既定的函数进行选择排序。
    /// </summary>
    /// <param name="sortRule">传入两个对象，若第一个对象应该排在前面，则返回 True。</param>
    public static List<T> Sort<T>(this IList<T> list, CompareThreadStart<T> sortRule)
    {
        List<T> newList = new List<T>();
        while (list.Any())
        {
            var highest = list[0];
            for (int i = 1; i <= list.Count - 1; i++)
            {
                if (sortRule(list[i], highest)) highest = list[i];
            }
            list.Remove(highest);
            newList.Add(highest);
        }
        return newList;
    }
    public delegate bool CompareThreadStart<T>(T left, T right);

    private class MyEqual<T>(CompareThreadStart<T> method) : IEqualityComparer<T>
    {
        public bool Equals(T? x, T? y)
        {
            return method(x, y);
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return obj.GetHashCode();
        }
    }

    /// <summary>
    /// 将元素与 List 的混合体拆分为元素组。
    /// </summary>
    public static List<T> GetFullList<T>(IList<T> data)
    {
        List<T> result = new List<T>();
        for (int i = 0; i <= data.Count - 1; i++)
        {
            if (data[i] is ICollection)
            {
                result.AddRange(data[i]);
            }
            else
            {
                result.Add(data[i]);
            }
        }
        return result;
    }

    /// <summary>
    /// 数组去重。
    /// </summary>
    public static List<T> Distinct<T>(this ICollection<T> arr, CompareThreadStart<T> isEqual)
    {
        return [..arr.Distinct(new MyEqual<T>(isEqual))];
    }

    /// <summary>
    /// 返回列表的浅表副本。
    /// </summary>
    public static IList<T> Clone<T>(this IList<T> list)
    {
        return (List<T>) [..list];
    }
}
