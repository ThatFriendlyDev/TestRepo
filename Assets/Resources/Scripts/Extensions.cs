using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public static class Extensions
{
    public static void SetListener(this UnityEvent uEvent, UnityAction call)
    {
        uEvent.RemoveAllListeners();
        uEvent.AddListener(call);
    }

    public static void SetListener<T>(this UnityEvent<T> uEvent, UnityAction<T> call)
    {
        uEvent.RemoveAllListeners();
        uEvent.AddListener(call);
    }

    public static List<U> FindDuplicates<T, U>(this List<T> list, Func<T, U> keySelector)
    {
        return list.GroupBy(keySelector)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key).ToList();
    }

    public static bool In<T>(this T value, params T[] values) => values.Contains(value);

    public static bool Contains(this string source, string toCheck, StringComparison comp)
    {
        return source != null && toCheck != null && source.IndexOf(toCheck, comp) >= 0;
    }

    public static T PopAt<T>(this IList<T> list, int index)
    {
        T r = list[index];
        list.RemoveAt(index);

        return r;
    }

    public static void Clear(this InputField inputfield)
    {
        inputfield.Select();
        inputfield.text = "";
    }

    public static string Parse(this Enum value)
    {
        return value.ToString().ToLower();
    }
}