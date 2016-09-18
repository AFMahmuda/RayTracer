using RayTracer.Common;
using RayTracer.Shape;
using RayTracer.Tracer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.BVH
{
    abstract class Container
    {
        protected List<SphereContainer> childs = new List<SphereContainer>();
        protected Geometry geo;
        public float area;

        public abstract bool IsIntersecting(Ray ray);
        public abstract Vector3 GetNormal(Point3 point);
    }



}
