﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public abstract class Geometry
    {
        private MyColor color;
        public MyColor Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public Transform Transform
        {
            get;
            set;
        }


        public Material Material
        {
            get;
            set;
        }

        public abstract bool CheckIntersection(Ray ray);
        public abstract Vector3 GetNormal(Point3 point);
        public MyColor Ambient { get; set; }


    }
}
