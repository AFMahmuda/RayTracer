using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public abstract class Geometry
    {
        private Color color;
        public Color Color
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

        public abstract bool CheckIntersection(Ray ray);
        public abstract Vector3 CalculateReflection(Ray ray);
        public abstract void transformToCameraSpace(Vector3 U, Vector3 V, Vector3 W);
    }
}
