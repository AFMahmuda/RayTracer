using RayTracer.Shape;
using RayTracer.Tracer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


            using (System.IO.StreamWriter file = new System.IO.StreamWriter("output.txt"))
            {
                foreach (Geometry item in scene.Geometries)
                {
                    string line =
                        Convert.ToString(item.GetMortonPos(), 2).PadLeft(30, '0') + "\t" +
                        item.Pos.X + "\t " +
                        item.Pos.Y + "\t " +
                        item.Pos.Z;
                    ;
                    file.WriteLine(line);
                }
            }

            List<SphereContainer> temp = new List<SphereContainer>();

            foreach (var item in scene.Geometries)
            {
                temp.Add(new SphereContainer(item));
            }

            while (temp.Count != 1)
            {
                float[,] area = new float[temp.Count, temp.Count];
                int smallestIndexI = -1;
                int smallestIndexJ = -1;
                float smallestVal = float.MaxValue;
                for (int i = 0; i < temp.Count; i++)
                {
                    for (int j = i + 1; j < temp.Count; j++)
                    {
                        area[i, j] = merge(temp[i], temp[j]).area;
                        if (area[i, j] < smallestVal)
                        {
                            smallestVal = area[i, j];
                            smallestIndexI = i;
                            smallestIndexJ = j;
                        }
                    }
                }
                temp.Add(new SphereContainer(temp[smallestIndexI], temp[smallestIndexJ]));
                temp.RemoveAt(smallestIndexI);
                temp.RemoveAt(smallestIndexJ);
            }
        }

        public SphereContainer merge(SphereContainer A, SphereContainer B)
        {
            return new SphereContainer(A, B);
        }


    }
}
