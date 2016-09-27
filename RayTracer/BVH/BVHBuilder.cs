using RayTracer.Algorithm;
using RayTracer.Shape;
using RayTracer.Tracer;
using System;
using System.Collections.Generic;

namespace RayTracer.BVH
{
    class BVHManager
    {
        public void BuildBVH(Scene scene)
        {
            foreach (Geometry item in scene.Geometries)
            {
                item.GetMortonPos();
            }

            scene.Geometries = RadixSort.Sort(scene.Geometries);

            List<Container> temp = new List<Container>();

            ContainerFactory conFac = new ContainerFactory();

            foreach (var item in scene.Geometries)
            {
                temp.Add(conFac.CreateContainer(item,Container.TYPE.BOX));
            }

            while (temp.Count > 1)
            {
                double[,] area = new double[temp.Count, temp.Count];
                int small_I = -1;
                int small_J = -1;
                Container small_C = null;
                double smallestVal = double.MaxValue;
                for (int i = 0; i < temp.Count; i++)
                {
                    for (int j = i + 1; j < temp.Count; j++)
                    {
                        Container tempCon = conFac.CombineContainer(temp[i], temp[j]);
                        area[i, j] = tempCon.area;
                        if (area[i, j] < smallestVal)
                        {
                            smallestVal = area[i, j];
                            small_I = i;
                            small_J = j;
                            small_C = tempCon;
                        }
                    }
                }
                Container delI = temp[small_I];
                Container delJ = temp[small_J];
                temp.Add(small_C);
                temp.Remove(delI);
                temp.Remove(delJ);
            }
            scene.Bvh = temp[0];
        }
    }
}
