using RayTracer.Shape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Algorithm
{
    class RadixSort
    {
        //TaskCompletionSource : https://en.wikibooks.org/wiki/Algorithm_Implementation/Sorting/Radix_sort
        public static List<Geometry> Sort(List<Geometry> a)
        {

            // our helper array 
            Geometry[] t = new Geometry[a.Count];

            // number of bits our group will be long 
            int r = 8; // try to set this also to 2, 8 or 16 to see if it is quicker or not 

            // number of bits of a C# int 
            int b = 32;

            // counting and prefix arrays
            // (note dimensions 2^r which is the number of all possible values of a r-bit number) 
            int[] count = new int[1 << r];
            int[] pref = new int[1 << r];

            // number of groups 
            int groups = (int)Math.Ceiling((double)b / (double)r);

            // the mask to identify groups 
            int mask = (1 << r) - 1;

            // the algorithm: 
            for (int c = 0, shift = 0; c < groups; c++, shift += r)
            {
                // reset count array 
                for (int j = 0; j < count.Length; j++)
                    count[j] = 0;

                // counting elements of the c-th group 
                for (int i = 0; i < a.Count; i++)
                    count[(a[i].GetMortonPos() >> shift) & mask]++;

                // calculating prefixes 
                pref[0] = 0;
                for (int i = 1; i < count.Length; i++)
                    pref[i] = pref[i - 1] + count[i - 1];

                // from a[] to t[] elements ordered by c-th group 
                for (int i = 0; i < a.Count; i++)
                    t[pref[(a[i].GetMortonPos() >> shift) & mask]++] = a[i];

                // a[]=t[] and start again until the last group 
                a = t.ToList();
            }
            // a is sorted 
            return a;
        }
    }
}
