using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Sphere : Geometry
    {

        public Sphere(Point3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        private Point3 center;
        private float radius;

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }


        public Point3 Center
        {
            get
            {
                return center;
            }
            set
            {
                center = value;

            }
        }

        public override bool IsIntersecting(Ray ray)
        {
            throw new NotImplementedException();
        }

        public override Vector3 CalculateReflection(Ray ray)
        {
            throw new NotImplementedException();
        }

    }
}
