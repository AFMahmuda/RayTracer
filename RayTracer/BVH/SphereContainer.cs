using RayTracer.Common;
using RayTracer.Shape;
using RayTracer.Tracer;
using RayTracer.Transformation;
using System;

namespace RayTracer.BVH
{
    class SphereContainer : Container
    {

        public Point3 center;
        public float radius;

        public SphereContainer(Geometry item)
        {
            geo = item;
        }

        public SphereContainer(SphereContainer a, SphereContainer b)
        {
            childs.Add(a);
            childs.Add(b);

            Vector3 aToB = new Vector3(a.center, b.center);
            radius = a.radius + b.radius + (float)aToB.Magnitude / 2f;
            center = (aToB.Normalize() * (radius - a.radius)).Point;
            area = (4f / 3f) * (float)(Math.PI * Math.Pow(radius, 3));
        }

        public override bool IsIntersecting(Ray ray)
        {


            Vector3 rayToSphere = new Vector3(ray.Start, center);

            //sphere is behind cam
            if (rayToSphere * ray.Direction <= 0)
                return false;

            Double a = ray.Direction * ray.Direction;
            Double b = -2 * (rayToSphere * ray.Direction);
            Double c = (rayToSphere * rayToSphere) - (radius * radius);
            Double dd = (b * b) - (4 * a * c);

            if (dd > 0)
            {
                Double res1 = (-b + Math.Sqrt(dd)) / (2.0 * a);
                Double res2 = (-b - Math.Sqrt(dd)) / (2.0 * a);
                Double distance;

                // if both results are negative, then the sphere is behind our ray, 
                // we already checked that.
                //if (res1 < 0 && res2 < 0)
                //    return false;
                //else
                distance = (res1 * res2 < 0) ? Math.Max(res1, res2) : Math.Min(res1, res2);
                if (ray.IsSmallerThanCurrent(distance, new Scaling()))
                {
                    ray.IntersectDistance = (ray.Direction * distance).Magnitude;
                    return true;
                }
            }
            return false;

        }

        public override Vector3 GetNormal(Point3 point)
        {
            Vector3 norm = new Vector3(center, point).Normalize();
            return norm;
        }
    }
}
