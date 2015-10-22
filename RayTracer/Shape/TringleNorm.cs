using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class TringleNorm : Geometry
    {

        public TringleNorm(Point3 a, Point3  b, Point3 c)
        {
            A_norm = a;
            B_norm = b;
            C_norm = c;
        }


        public Point3 A_norm
        { get; set; }

        public Point3 B_norm
        { get; set; }

        public Point3 C_norm
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
