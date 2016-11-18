using RayTracer.Algorithm;
using RayTracer.Shape;
using RayTracer.Tracer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            for (int i = 0; i < scene.Geometries.Length; i++)
            {
                scene.Geometries[i].GetMortonPos();
            }

            scene.Geometries = RadixSort.Sort(scene.Geometries.ToArray());

            List<Container> temp = new List<Container>();
            if (!isAAC)
                for (int i = 0; i < scene.Geometries.Length; i++)
                {
                    temp.Add(ContainerFactory.Instance.CreateContainer(scene.Geometries[i], type));

                }
            else
                temp = BuildTree(scene.Geometries);

            temp = CombineCluster(temp, 1);
            scene.Bvh = temp[0];
        }


        int threshold = 4; //4 or 20
        List<Container> BuildTree(Geometry[] primitives)
        {
            List<Container> bins = new List<Container>();
            if (primitives.Length < threshold)
            {

                for (int i = 0; i < primitives.Length; i++)
                {
                    bins.Add(ContainerFactory.Instance.CreateContainer(primitives[i], type));
                }
                return CombineCluster(bins, f(threshold));
            }

            int pivot = getPivot(primitives);
            Geometry[] left = primitives.Take(pivot).ToArray();
            Geometry[] right = primitives.Skip(pivot).ToArray(); // pivot included in right

            List<Container> leftTree = new List<Container>(), rightTree = new List<Container>();

            Task[] BuildTreeTask = new Task[2];
            BuildTreeTask[0] = Task.Factory.StartNew(() => leftTree = BuildTree(left).ToList());
            BuildTreeTask[1] = Task.Factory.StartNew(() => rightTree = BuildTree(right).ToList());
            Task.WaitAll(BuildTreeTask);


            bins.AddRange(leftTree);
            bins.AddRange(rightTree);
            //bins.AddRange(BuildTree(left));
            //bins.AddRange(BuildTree(right));
            return CombineCluster(bins, f(bins.Count));
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
        int getPivot(Geometry[] geo)
        {
            for (int i = 0; i < 30; i++)
            {
                for (int j = 1; j < geo.Length; j++)
                {
                    string last = geo[j - 1].GetMortonBitString();
                    string curr = geo[j].GetMortonBitString();
                    if (curr[i] != last[i])
                        return j;
                }
            }
            return geo.Length / 2;
        }


        //combine bins cluster to [limit] cluster
        List<Container> CombineCluster(List<Container> bins, int limit)
        {
            //Console.WriteLine("Combining from " + bins.Count + " to " + limit);

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
