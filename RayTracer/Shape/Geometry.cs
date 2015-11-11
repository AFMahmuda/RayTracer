using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RayTracer
{
    [Serializable]
    public abstract class Geometry
    {
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

        public abstract bool IsIntersecting(Ray ray);
        public abstract Vector3 GetNormal(Point3 point);
        public MyColor Ambient { get; set; }


    }
}
