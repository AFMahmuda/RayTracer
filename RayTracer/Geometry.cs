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

        public abstract bool IsIntersecting(Ray ray);
        public abstract Vector3 CalculateReflection(Ray ray);        
    }
}
