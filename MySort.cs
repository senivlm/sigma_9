using System;
using System.Collections.Generic;
using System.Text;

namespace sigma_9
{
    public delegate int Compare(object obj1, object obj2);
   
    static class MySort
    {
        public static void SortProducts(object[] arr, Compare compare)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
                for (int j = 0; j < n - i - 1; j++)
                    if (compare(arr[j], arr[j+1])>0)
                    {
                        // swap temp and arr[i]
                        object temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
        }
    }
}
