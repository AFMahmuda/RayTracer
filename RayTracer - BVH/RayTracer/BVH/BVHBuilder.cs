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
        }
    }
}
