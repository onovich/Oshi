using UnityEngine;
using System.Runtime.CompilerServices;

namespace Oshi {

    public static class GLog {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Log(string msg) {
            Debug.Log(msg);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogWarning(string msg) {
            Debug.LogWarning(msg);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogError(string msg) {
            Debug.LogError(msg);
        }

    }

}