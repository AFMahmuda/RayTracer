using System;
using RayTracer.Common;
using RayTracer.Tracer;

namespace RayTracer.BVH
{
   public class BoxContainer : Container
    {

        Point3 min;
        Point3 max;

        public override bool IsIntersecting(Ray ray)
        {
            double tmin = double.MinValue, tmax = double.MaxValue;

            if (ray.Direction.Point.X != 0.0)
            {
                double tx1 = (min.X - ray.Start.X) / ray.Direction.Point.X;
                double tx2 = (max.X - ray.Start.X) / ray.Direction.Point.X;

                tmin = Math.Max(tmin, Math.Min(tx1, tx2));
                tmax = Math.Min(tmax, Math.Max(tx1, tx2));
            }
            if (ray.Direction.Point.Y != 0.0)
            {
                double tx1 = (min.Y - ray.Start.Y) / ray.Direction.Point.Y;
                double tx2 = (max.Y - ray.Start.Y) / ray.Direction.Point.Y;

                tmin = Math.Max(tmin, Math.Min(tx1, tx2));
                tmax = Math.Min(tmax, Math.Max(tx1, tx2));
            }

            if (ray.Direction.Point.Z != 0.0)
            {
                double tx1 = (min.Z - ray.Start.Z) / ray.Direction.Point.Z;
                double tx2 = (max.Z - ray.Start.Z) / ray.Direction.Point.Z;

                tmin = Math.Max(tmin, Math.Min(tx1, tx2));
                tmax = Math.Min(tmax, Math.Max(tx1, tx2));
            }


            return tmax >= tmin;
        }

    }
}
