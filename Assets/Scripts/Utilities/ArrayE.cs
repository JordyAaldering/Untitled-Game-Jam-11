using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utilities
{
    public static class ArrayE
    {
        public static T GetClamped<T>(this T[] arr, int i)
        {
            if (arr == null || arr.Length == 0)
                return default;
            return arr[Mathf.Clamp(i, 0, arr.Length - 1)];
        }
        
        public static T ClampBounds<T>(this T[] arr, int i, T min, T max)
        {
            if (arr == null || arr.Length == 0)
                return default;
            return i < 0 ? min : i > arr.Length ? max : arr[i];
        }
        
        public static void Shuffle<T>(this T[] arr)
        {
            int n = arr.Length;
            while (n > 1) 
            {
                int k = Random.Range(0, n--);
                T temp = arr[n];
                arr[n] = arr[k];
                arr[k] = temp;
            }
        }
    }
}
