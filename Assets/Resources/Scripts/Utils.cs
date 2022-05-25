using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utils
{
    private static readonly SortedDictionary<int, string> abbrevations = new SortedDictionary<int, string>
    {
        { 1000, "K" },
        { 1000000, "M" },
        { 1000000000, "B" }
    };

    public static Vector2 SwitchToRectTransform(RectTransform from, RectTransform to)
    {
        Vector2 fromPivotDerivedOffset = new Vector2(from.rect.width * 0.5f + from.rect.xMin, from.rect.height * 0.5f + from.rect.yMin);
        Vector2 screenP = RectTransformUtility.WorldToScreenPoint(null, from.position);
        screenP += fromPivotDerivedOffset;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(to, screenP, null, out Vector2 localPoint);
        Vector2 pivotDerivedOffset = new Vector2(to.rect.width * 0.5f + to.rect.xMin, to.rect.height * 0.5f + to.rect.yMin);

        return to.anchoredPosition + localPoint - pivotDerivedOffset;
    }

    public static string GetAnonymousPlayerName()
    {
        int number = UnityEngine.Random.Range(999, 9999);

        return "Player#" + number.ToString();
    }

    public static string GetRandomString(int length)
    {
        string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
        var builder = new StringBuilder();
        var random = new System.Random();

        for (var i = 0; i < length; i++)
        {
            var c = pool[random.Next(0, pool.Length)];
            builder.Append(c);
        }

        return builder.ToString();
    }

    public static string GetFormattedTime(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);

        return time.ToString(@"mm\:ss");
    }

    public static bool IsCursorOverUserInterface()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
            {
                return true;
            }
        }

        return GUIUtility.hotControl != 0;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static int GetTimestamp()
    {
        return (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
    }

    public static object GetFieldValue(object obj, string propertyName)
    {
        return obj.GetType().GetField(propertyName).GetValue(obj);
    }

    public static int GetRandomChance()
    {
        return UnityEngine.Random.Range(0, 100);
    }

    public static Vector3 GetTopCenterPosition(Collider collider)
    {
        Vector3 centerPosition = collider.bounds.center;
        Vector3 topPosition = collider.bounds.max;

        return new Vector3(centerPosition.x, topPosition.y, centerPosition.z);
    }

    public static string SplitStringIntoWords(string rawName)
    {
        string[] title = Regex.Matches(rawName, @"([A-Z][a-z]+)")
              .Cast<Match>()
              .Select(m => m.Value).ToArray();

        return string.Join(" ", title);
    }

    public static bool IsBetween(double testValue, double bound1, double bound2)
    {
        return (testValue >= Math.Min(bound1, bound2) && testValue <= Math.Max(bound1, bound2));
    }

    public static Vector3 GetScreenPosition(Camera camera, Vector3 targetPosition)
    {
        return camera.WorldToScreenPoint(targetPosition);
    }

    public static bool IsTargetVisible(Vector3 screenPosition)
    {
        return screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < Screen.width && screenPosition.y > 0 && screenPosition.y < Screen.height;
    }

    public static void GetArrowIndicatorPositionAndAngle(ref Vector3 screenPosition, ref float angle, Vector3 screenCentre, Vector3 screenBounds)
    {
        screenPosition -= screenCentre;

        if (screenPosition.z < 0)
        {
            screenPosition *= -1;
        }

        angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
        float slope = Mathf.Tan(angle);

        if (screenPosition.x > 0)
        {
            screenPosition = new Vector3(screenBounds.x, screenBounds.x * slope, 0);
        }
        else
        {
            screenPosition = new Vector3(-screenBounds.x, -screenBounds.x * slope, 0);
        }

        if (screenPosition.y > screenBounds.y)
        {
            screenPosition = new Vector3(screenBounds.y / slope, screenBounds.y, 0);
        }
        else if (screenPosition.y < -screenBounds.y)
        {
            screenPosition = new Vector3(-screenBounds.y / slope, -screenBounds.y, 0);
        }

        screenPosition += screenCentre;
    }

    public static string AbbreviateNumber(float number)
    {
        if (number < 10000)
        {
            return number.ToString();
        }

        for (int i = abbrevations.Count - 1; i >= 0; i--)
        {
            KeyValuePair<int, string> pair = abbrevations.ElementAt(i);

            if (Mathf.Abs(number) >= pair.Key)
            {
                int roundedNumber = Mathf.FloorToInt(number / pair.Key);

                return roundedNumber.ToString() + pair.Value;
            }
        }

        return number.ToString();
    }

    public static Vector3 GetPositionAroundObject(Vector3 position, float radius)
    {
        Vector3 offset = UnityEngine.Random.insideUnitCircle * radius;

        return position + new Vector3(offset.x, 0, offset.y);
    }
}