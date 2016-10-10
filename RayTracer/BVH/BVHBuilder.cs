using RayTracer.Algorithm;
using RayTracer.Shape;
using RayTracer.Tracer;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer.BVH
{
    class BVHManager
    {
        bool isAAC;
        Container.TYPE type;

        public BVHManager(Container.TYPE _type = Container.TYPE.BOX, bool _isAAC = true)
        {
            type = _type;
            isAAC = _isAAC;
        }


        public void BuildBVH(Scene scene)
        {

            for (int i = 0; i < scene.Geometries.Count; i++)
            {
                scene.Geometries[i].GetMortonPos();
            }

            scene.Geometries = RadixSort.Sort(scene.Geometries);

            List<Container> temp = new List<Container>();
            if (!isAAC)
                for (int i = 0; i < scene.Geometries.Count; i++)
                {
                    temp.Add(ContainerFactory.Instance.CreateContainer(scene.Geometries[i], type));

                }
            else
                temp = BuildTree(scene.Geometries);

            temp = CombineCluster(temp, 1);
            scene.Bvh = temp[0];
        }


        int threshold = 4; //4 or 20
        List<Container> BuildTree(List<Geometry> primitives)
        {
            List<Container> bins = new List<Container>();
            if (primitives.Count < threshold)
            {
                for (int i = 0; i < primitives.Count; i++)
                {
                    bins.Add(ContainerFactory.Instance.CreateContainer(primitives[i], type));
                }

                return CombineCluster(bins, f(threshold));
            }


            int pivot = getPivot(primitives);
            List<Geometry> left = primitives.GetRange(0, pivot);
            List<Geometry> right = primitives.GetRange(pivot, primitives.Count - pivot); // pivot included in right

            bins.AddRange(BuildTree(left));
            bins.AddRange(BuildTree(right));

            return CombineCluster(bins, f(primitives.Count));
        }

        /*cluster reduction function
         * n -> number of input clusters
         * f(n) -> number of max output cluster
        */

        int f(int n)
        {
            return n / 2;
        }

        /*list partition function, 
         * pivot is the frst bit 'flip'
         * ex : [0] 00000111
         *      [1] 00001000
         *      [2] 00001000
         *      [3] 00100000
         *      pivot -> 3 (flipped on third element 000xxxxx to 001xxxxx)
        */
        int getPivot(List<Geometry> geo)
        {
            for (int i = 0; i < 30; i++)
            {
                for (int j = 1; j < geo.Count; j++)
                {
                    BitArray last = geo[j].mortonBit;
                    BitArray curr = geo[j - 1].mortonBit;
                    if (last[i] != curr[i])
                        return j;
                }
            }
            return geo.Count / 2;
        }


        //combine bins cluster to [limit] cluster
        List<Container> CombineCluster(List<Container> bins, int limit)
        {

            for (int i = 0; i < bins.Count; i++)
            {

                bins[i].FindBestMatch(bins);
            }

            while (bins.Count > limit)
            {
                float bestDist = float.MaxValue;
                Container left = null;
                Container right = null;

                for (int i = 0; i < bins.Count; i++)
                {
                    if (bins[i].areaWithClosest < bestDist)
                    {
                        bestDist = bins[i].areaWithClosest;
                        left = bins[i];
                        right = bins[i].closest;
                    }
                }


                Container newBin = ContainerFactory.Instance.CombineContainer(left, right);
                bins.Remove(left);
                bins.Remove(right);
                bins.Add(newBin);

                newBin.FindBestMatch(bins);
                for (int i = 0; i < bins.Count; i++)
                {
                    if (bins[i].closest == left || bins[i].closest == right)
                        bins[i].FindBestMatch(bins);
                }
            }
            return bins;
        }
    }
}
