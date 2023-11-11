using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alter {

    public static class Vector2Extension {

        public static Vector3 ToVector3(this Vector2 vector2) {
            var vector3 = new Vector3(vector2.x, vector2.y, 0);
            return vector3;
        }

    }

    public static class Vector2IntExtension {

        public static Vector3 ToVector3(this Vector2Int vector2) {
            var vector3 = new Vector3(vector2.x, vector2.y, 0);
            return vector3;
        }

    }

    public static class Vector3Extension {

        public static Vector2 ToVector2(this Vector3 vector3) {
            var vector2 = new Vector2(vector3.x, vector3.y);
            return vector2;
        }

    }

    public static class SortedListExtension {

        public static TValue GetOrAdd<TKey, TValue>(this SortedList<TKey, TValue> sortedList, TKey key, Func<TValue> valueFactory) {
            if (!sortedList.TryGetValue(key, out TValue value)) {
                value = valueFactory();
                sortedList.Add(key, value);
            }
            return value;
        }

    }

    public static class SortedDictionaryExtension {

        public static TValue GetOrAdd<TKey, TValue>(this SortedDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory) {
            if (!dictionary.TryGetValue(key, out TValue value)) {
                value = valueFactory();
                dictionary.Add(key, value);
            }
            return value;
        }

    }

    public static class DictionaryExtension {

        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory) {
            if (!dictionary.TryGetValue(key, out TValue value)) {
                value = valueFactory();
                dictionary.Add(key, value);
            }
            return value;
        }

    }

    public static class FloatExtension {

        public static string ToSignString(this float value, int decimalPlaces = 2) {
            if (value > 0) {
                return "+ " + value.ToString("F" + decimalPlaces.ToString());
            }
            return value.ToString("F" + decimalPlaces.ToString());
        }

        public static string ToFixedDecimalString(this float value, int decimalPlaces = 2) {
            return value.ToString("F" + decimalPlaces.ToString());
        }

        public static string ToFixedPercentString(this float value, int decimalPlaces = 2) {
            return value.ToString("F" + decimalPlaces.ToString()) + " %";
        }

    }

    public static class IntExtension {

        public static string ToSignString(this int value) {
            if (value > 0) {
                return "+ " + value;
            }
            return value.ToString();
        }

    }

}