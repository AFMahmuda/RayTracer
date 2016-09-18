using RayTracer.Shape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayTracer.Common;
using RayTracer.Tracer;

namespace RayTracer.BVH
{
    class BoxContainer : Container
    {
        public override Vector3 GetNormal(Point3 point)
        {
            throw new NotImplementedException();
        }

        public override bool IsIntersecting(Ray ray)
        {
            throw new NotImplementedException();
        }
    }
}
