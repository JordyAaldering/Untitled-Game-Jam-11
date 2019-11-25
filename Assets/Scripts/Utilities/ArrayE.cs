using System;

namespace Utilities
{
    public static class ArrayE
    {
        public static float GetClosest(this float[] arr, float target)
        {
            int n = arr.Length;
            
            if (target <= arr[0])
                return arr[0];
            
            if (target >= arr[n - 1])
                return arr[n - 1];
            
            int i = 0, j = n, mid = 0; 
            while (i < j) 
            { 
                mid = (i + j) / 2;
                
                if (Math.Abs(arr[mid] - target) < float.Epsilon) 
                    return arr[mid]; 
  
                if (target < arr[mid])
                {
                    if (mid > 0 && target > arr[mid - 1])
                        return target - arr[mid - 1] < arr[mid] - target ? arr[mid - 1] : arr[mid];
                  
                    j = mid;              
                }
                else 
                { 
                    if (mid < n-1 && target < arr[mid + 1])  
                        return target - arr[mid] < arr[mid + 1] - target ? arr[mid] : arr[mid + 1];
                    
                    i = mid + 1;
                } 
            }
            
            return arr[mid]; 
        }
        
        public static int GetClosestIndex(this float[] arr, float target)
        {
            int n = arr.Length;
            
            if (target <= arr[0])
                return 0;
            
            if (target >= arr[n - 1])
                return n - 1;
            
            int i = 0, j = n, mid = 0; 
            while (i < j) 
            { 
                mid = (i + j) / 2;
                
                if (Math.Abs(arr[mid] - target) < float.Epsilon) 
                    return mid;
  
                if (target < arr[mid])
                {
                    if (mid > 0 && target > arr[mid - 1])
                        return target - arr[mid - 1] < arr[mid] - target ? mid - 1 : mid;
                  
                    j = mid;              
                }
                else 
                { 
                    if (mid < n-1 && target < arr[mid + 1])  
                        return target - arr[mid] < arr[mid + 1] - target ? mid : mid + 1;
                    
                    i = mid + 1;
                } 
            }
            
            return mid; 
        }
    }
}
