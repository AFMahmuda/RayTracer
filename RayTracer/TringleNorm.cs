using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class TringleNorm : Triangle
    {
        public Point3 A_norm
        { get; set; }

        public Point3 B_norm
        { get; set; }

        public Point3 C_norm
        { get; set; }

        public override Vector3 CalculateReflection(Ray ray)
        {
            throw new NotImplementedException();
        }

    }
}
