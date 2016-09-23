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
    public abstract class Container
    {
        public enum TYPE
        {
            BOX,
            SPHERE
        }
        public TYPE Type;

        protected List<SphereContainer> childs = new List<SphereContainer>();
        protected Geometry geo = null;
        public double area;

        public abstract bool IsIntersecting(Ray ray);
        public Geometry Geo { get { return geo; } }
        public List<SphereContainer> Childs { get { return childs; } }
    }
}
