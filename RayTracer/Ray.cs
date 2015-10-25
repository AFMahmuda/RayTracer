﻿using System;
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
            IntersectDistance = float.MaxValue;
            IntersectWith = null;
        }
        public Point3 Start
        { get; set; }

        public Vector3 Direction
        { get; set; }



        public Color Color
        { get; set; }

        public float IntersectDistance
        { get; set; }

        public Geometry IntersectWith
        { get; set; }

        public Color Trace(Scene scene, int bounce)
        {
            if (bounce > scene.MaxDepth)
                return Color;

            foreach (var item in scene.Geometries)
            {

                
                Direction = Matrix.Mult44x41(item.Transform.Matrix.Inverse4X4(), Direction);
                Direction /= Direction.Magnitude;
                //              Direction.ShowInformation();
                if (item.CheckIntersection(this))
                    IntersectWith = item;

                Direction = Matrix.Mult44x41(item.Transform.Matrix, Direction);
                Direction /= Direction.Magnitude;
                //                Direction.ShowInformation();
            }

            if (IntersectWith != null)
                Color = Color.CadetBlue;
            //Direction = IntersectWith.CalculateReflection(this);
            //Start = Start + (Direction * IntersectDistance).Value;
            //return Trace(scene, bounce++);

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
