using System;
using System.Globalization;
using JetBrains.Annotations;
using UnityEngine;

namespace CAFU.KeyValueStore.Application.Utility
{
    [PublicAPI]
    public static class Serializer
    {
        public static string Default<T>(T value)
        {
            return JsonUtility.ToJson(value);
        }

        public static string DateTime(DateTime value)
        {
            // ReSharper disable once StringLiteralTypo
            return value.ToString("yyyyMMddHHmmssfff");
        }
    }

    [PublicAPI]
    public static class Deserializer
    {
        public static T Default<T>(string value)
        {
            return JsonUtility.FromJson<T>(value);
        }

        public static DateTime DateTime(string value)
        {
            // ReSharper disable once StringLiteralTypo
            return System.DateTime.ParseExact(value, "yyyyMMddHHmmssfff", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);
        }
    }
}