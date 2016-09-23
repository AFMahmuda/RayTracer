using RayTracer.Algorithm;
using RayTracer.Shape;
using RayTracer.Tracer;
using System;
using System.Collections.Generic;

namespace RayTracer.BVH
{
    class BVHBuilder
    {
        public void BuildBVH(Scene scene)
        {
            foreach (Geometry item in scene.Geometries)
            {
                item.GetMortonPos();
            }

            scene.Geometries = RadixSort.Sort(scene.Geometries);


            //using (System.IO.StreamWriter file = new System.IO.StreamWriter("output.txt"))
            //{
            //    foreach (Geometry item in scene.Geometries)
            //    {
            //        string line =
            //            Convert.ToString(item.GetMortonPos(), 2).PadLeft(30, '0') + "\t" +
            //            item.Pos.X + "\t " +
            //            item.Pos.Y + "\t " +
            //            item.Pos.Z;
            //        ;
            //        file.WriteLine(line);
            //    }
            //}

            List<SphereContainer> temp = new List<SphereContainer>();

            foreach (var item in scene.Geometries)
            {
                temp.Add(new SphereContainer(item));
            }

            while (temp.Count > 1)
            {
                double[,] area = new double[temp.Count, temp.Count];
                int small_I = -1;
                int small_J = -1;
                double smallestVal = double.MaxValue;
                for (int i = 0; i < temp.Count; i++)
                {
                    for (int j = i + 1; j < temp.Count; j++)
                    {
                        area[i, j] = merge(temp[i], temp[j]).area;
                        if (area[i, j] < smallestVal)
                        {
                            smallestVal = area[i, j];
                            small_I = i;
                            small_J = j;
                        }
                    }
                }
                temp.Add(new SphereContainer(temp[small_I], temp[small_J]));
                SphereContainer delI = temp[small_I];
                SphereContainer delJ = temp[small_J];
                temp.Remove(delI);
                temp.Remove(delJ);
            }
            scene.Bvh = temp[0];
        }


        public Container merge(SphereContainer A, SphereContainer B)
        {
            return new SphereContainer(A, B);
        }


    }
}
