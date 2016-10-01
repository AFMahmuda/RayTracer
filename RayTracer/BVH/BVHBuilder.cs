using RayTracer.Algorithm;
using RayTracer.Shape;
using RayTracer.Tracer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace RayTracer.BVH
{
    class BVHManager
    {
        public void BuildBVH(Scene scene)
        {
            foreach (Geometry item in scene.Geometries)
                item.GetMortonPos();

            scene.Geometries = RadixSort.Sort(scene.Geometries);

            List<Container> temp = new List<Container>();
            //foreach (var item in scene.Geometries)
            //    temp.Add(ContainerFactory.Instance.CreateContainer(item, Container.TYPE.BOX));

            temp = BuildTree(scene.Geometries);

            temp = CombineCluster(temp, 1);
            scene.Bvh = temp[0];
        }


        int threshold = 4; //4 or 20
        List<Container> BuildTree(List<Geometry> primitives)
        {
            List<Container> bins = new List<Container>();
            if (primitives.Count > threshold)
            {
                foreach (Geometry item in primitives)
                    bins.Add(ContainerFactory.Instance.CreateContainer(item, Container.TYPE.BOX));
                return CombineCluster(bins, f(threshold));
            }


            int pivot = getPivot(primitives);
            List<Geometry> left = primitives.GetRange(0, pivot); // pivot included in left
            List<Geometry> right = primitives.GetRange(pivot + 1, primitives.Count - pivot);

            bins.AddRange(BuildTree(left));
            bins.AddRange(BuildTree(right));

            return CombineCluster(bins, f(primitives.Count));
        }

        //cluster reduction function
        int f(int n)
        {
            return n / 2;
        }

        //list partition function
        int getPivot(List<Geometry> primitives)
        {


            for (int i = 0; i < 30; i++)
            {
                for (int j = 1; j < primitives.Count; j++)
                {
                    BitArray last = primitives[j].mortonBit;
                    BitArray curr = primitives[j - 1].mortonBit;
                    if (last[i] != curr[i])
                        return j;
                }
            }
            return primitives.Count / 2;
        }

        List<Container> CombineCluster(List<Container> bins, int n)
        {
            foreach (Container item in bins)
            {
                item.FindBestMatch(bins);
            }

            while (bins.Count > n)
            {
                float bestDist = float.MaxValue;
                Container left = null;
                Container right = null;

                foreach (Container item in bins)
                {
                    if (item.areaWithClosest < bestDist)
                    {
                        bestDist = item.areaWithClosest;
                        left = item;
                        right = item.closest;
                    }
                }

                Container newBin = ContainerFactory.Instance.CombineContainer(left, right);
                bins.Remove(left);
                bins.Remove(right);
                bins.Add(newBin);

                newBin.FindBestMatch(bins);
                foreach (Container item in bins)
                {
                    if (item.closest == left || item.closest == right)
                        item.FindBestMatch(bins);
                }
            }
            return bins;
        }
    }


}
