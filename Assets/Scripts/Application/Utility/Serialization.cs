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

        public static string ReferenceType<T>(T value) where T : class
        {
            return JsonUtility.ToJson(value);
        }

        public static string ValueType<T>(ReferenceWrapper<T> value) where T : struct
        {
            return JsonUtility.ToJson(value.Unwrap());
        }

        public static string DateTime(ReferenceWrapper<DateTime> value)
        {
            // ReSharper disable once StringLiteralTypo
            return value.Unwrap().ToString("yyyyMMddHHmmssfff");
        }
    }

    [PublicAPI]
    public static class Deserializer
    {
        public static T Default<T>(string value)
        {
            return JsonUtility.FromJson<T>(value);
        }

        public static T ReferenceType<T>(string value) where T : class
        {
            return JsonUtility.FromJson<T>(value);
        }

        public static ReferenceWrapper<T> ValueType<T>(string value) where T : struct
        {
            return JsonUtility.FromJson<T>(value).Wrap();
        }

        public static ReferenceWrapper<DateTime> DateTime(string value)
        {
            // ReSharper disable once StringLiteralTypo
            return System.DateTime.ParseExact(value, "yyyyMMddHHmmssfff", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None).Wrap();
        }
    }
}