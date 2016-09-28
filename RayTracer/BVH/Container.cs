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
    [Serializable]
    public abstract class Container
    {
        public enum TYPE
        {
            BOX,
            SPHERE
        }
        public TYPE Type;

        protected List<Container> childs = new List<Container>();
        protected Geometry geo = null;
        public float area;

        public abstract bool IsIntersecting(Ray ray);
        public Geometry Geo { get { return geo; } }
        public List<Container> Childs { get { return childs; } }
    }
}
