using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanogramSolver
{
    public static class StaticExtensionsMethod
    {

            public static void Fill<T>(this T[] originalArray, T with)
            {
                for (int i = 0; i < originalArray.Length; i++)
                {
                    originalArray[i] = with;
                }
            }

        public static void ReverseArray<T>(this T[] array)
        {
            int i = 0;
            int j = array.Length - 1;
            while (i < j)
            {
                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
                i++;
                j--;
            }
        }

        public static void NumberArray(this int[] array)
        {
            int empty = 0;
            int full = 1;
            if (array[0] == 1)
                empty--;
            for (int i = 0; i < array.Length-1; i++)
            {
                if (array[i] == 0)
                {
                    array[i] = empty;
                    if (array[i + 1] == 1)
                        empty--;
                }
                else
                {
                    array[i] = full;
                    if (array[i + 1] == 0)
                        full++;
                }
            }
            if (array[array.Length - 1] == 0)
                array[array.Length - 1] = empty;
            else
                array[array.Length - 1] = full;
        }

    }
}
