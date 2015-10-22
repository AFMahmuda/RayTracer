using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Ray
    {
        public Point3 Start
        { get; set; }

        public Vector3 Direction
        { get; set; }

        public Point3 Intersection
        { get; set; }

        public Color Color
        { get; set; }

        public Color Trace(Scene scene, int bounce)
        {
            if (bounce > scene.MaxDepth)
                return Color;
            Intersection = (Direction * float.MaxValue).End;

            
            Color = Color.FromArgb(bounce,bounce,bounce);
            foreach (var item in scene.Geometries)
            {

                if (item.IsIntersecting(this))
                {
                    Direction = item.CalculateReflection(this);
                    Start = Intersection;
                    Trace(scene, bounce++);
                }

            }

            Color = Color.FromArgb(bounce, bounce, bounce);
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
