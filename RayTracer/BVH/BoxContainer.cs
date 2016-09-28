using System;
using RayTracer.Common;
using RayTracer.Shape;
using RayTracer.Tracer;

namespace RayTracer.BVH
{
    [Serializable]
    public class BoxContainer : Container
    {

        private Point3 min;
        private Point3 max;

        public BoxContainer(Geometry item)
        {
            Type = TYPE.BOX;
            this.geo = item;

            if (item.GetType() == typeof(Sphere))
            {
                Sphere sphere = (Sphere)item;
                min = Utils.DeepClone(sphere.center);
                min.X -= sphere.radius;
                min.Y -= sphere.radius;
                min.Z += sphere.radius;


                max = Utils.DeepClone(sphere.center);
                max.X += sphere.radius;
                max.Y += sphere.radius;
                max.Z -= sphere.radius;
                min = Mattrix.Mul44x41(item.Trans.Matrix, new Vec3(min), 1).Point;
                max = Mattrix.Mul44x41(item.Trans.Matrix, new Vec3(max), 1).Point;
            }

            else //if (item.GetType() == typeof(Triangle))
            {
                Triangle tri = (Triangle)item;

                Point3 a = Mattrix.Mul44x41(item.Trans.Matrix, new Vec3(tri.a), 1).Point;
                Point3 b = Mattrix.Mul44x41(item.Trans.Matrix, new Vec3(tri.b), 1).Point;
                Point3 c = Mattrix.Mul44x41(item.Trans.Matrix, new Vec3(tri.c), 1).Point;

                min = new Point3();
                min.X = Math.Min(a.X, Math.Min(b.X, c.X));
                min.Y = Math.Min(a.Y, Math.Min(b.Y, c.Y));
                min.Z = Math.Max(a.Z, Math.Max(b.Z, c.Z));

                max = new Point3();
                max.X = Math.Max(a.X, Math.Max(b.X, c.X));
                max.Y = Math.Max(a.Y, Math.Max(b.Y, c.Y));
                max.Z = Math.Min(a.Z, Math.Min(b.Z, c.Z));
            }
        }

        public BoxContainer(BoxContainer a, BoxContainer b)
        {
            Type = TYPE.BOX;
            childs.Add(a);
            childs.Add(b);

            min = new Point3();
            min.X = Math.Min(a.min.X, b.min.X);
            min.Y = Math.Min(a.min.Y, b.min.Y);
            min.Z = Math.Max(a.min.Z, b.min.Z);

            max = new Point3();
            max.X = Math.Max(a.max.X, b.max.X);
            max.Y = Math.Max(a.max.Y, b.max.Y);
            max.Z = Math.Min(a.max.Z, b.max.Z);

            Point3 size = max - min;

            area = 2f * (size.X * size.Y + size.X * (size.Z * -1) + size.X * (size.Z * -1));

        }

        public override bool IsIntersecting(Ray ray)
        {
            float tmin = float.MinValue, tmax = float.MaxValue;

            if (ray.Direction.Point.X != 0.0)
            {
                float tx1 = (min.X - ray.Start.X) / ray.Direction.Point.X;
                float tx2 = (max.X - ray.Start.X) / ray.Direction.Point.X;

                tmin = Math.Max(tmin, Math.Min(tx1, tx2));
                tmax = Math.Min(tmax, Math.Max(tx1, tx2));
            }
            if (ray.Direction.Point.Y != 0.0)
            {
                float ty1 = (min.Y - ray.Start.Y) / ray.Direction.Point.Y;
                float ty2 = (max.Y - ray.Start.Y) / ray.Direction.Point.Y;

                tmin = Math.Max(tmin, Math.Min(ty1, ty2));
                tmax = Math.Min(tmax, Math.Max(ty1, ty2));
            }

            if (ray.Direction.Point.Z != 0.0)
            {
                float tz1 = (min.Z - ray.Start.Z) / ray.Direction.Point.Z;
                float tz2 = (max.Z - ray.Start.Z) / ray.Direction.Point.Z;

                tmin = Math.Max(tmin, Math.Min(tz1, tz2));
                tmax = Math.Min(tmax, Math.Max(tz1, tz2));
            }

            return tmax >= tmin;

        }

    }
}
