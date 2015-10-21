using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class DirectionalLight : Light
    {
        Point3 direction;
        public Point3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }
    }
}
