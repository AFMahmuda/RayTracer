using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Ray
    {
        public Ray()
        {
            Color = Color.FromArgb(0, 0, 0);
            Intersection = float.MaxValue;
        }
        public Point3 Start
        { get; set; }

        public Vector3 Direction
        { get; set; }

        public float Intersection
        { get; set; }

        public Color Color
        { get; set; }

        public Color Trace(Scene scene, int bounce)
        {
            if (bounce > scene.MaxDepth)
                return Color;


            foreach (var item in scene.Geometries)
            {

                if (item.CheckIntersection(this))
                {
                    Color = Color.CadetBlue;
                    //Direction = item.CalculateReflection(this);
                    //Start = Start + Direction.End * Intersection;
                    //Trace(scene, bounce++);
                }
            }

            return Color;
        }


        internal void ShowInformation()
        {
            Console.WriteLine("Ray Information======================================");
            Console.Write("Start : ");
            Start.ShowInformation();
            Console.WriteLine("Direction");
            Direction.ShowInformation();
            Console.WriteLine("=====================================================");
        }

    }
}
