using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Triangle : Geometry
    {
        public Point3 A
        { get; set; }

        public Point3 B
        { get; set; }

        public Point3 C
        { get; set; }

        public override bool IsIntersecting(Ray ray)
        {
            throw new NotImplementedException();
        }

        public override Vector3 CalculateReflection(Ray ray)
        {
            return new Vector3(ray.Intersection, ray.Intersection);
        }
    }
}
