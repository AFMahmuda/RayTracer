using System;
using RayTracer.Common;
using RayTracer.Shape;
using RayTracer.Tracer;

namespace RayTracer.BVH
{
    [Serializable]
    public class BoxContainer : Container
    {

        Point3 min;
        Point3 max;


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
                min = MyMat.Mul44x41(item.Trans.Matrix, new Vector3(min), 1).Point;
                max = MyMat.Mul44x41(item.Trans.Matrix, new Vector3(max), 1).Point;
            }

            else //if (item.GetType() == typeof(Triangle))
            {
                Triangle tri = (Triangle)item;

                Point3 a = MyMat.Mul44x41(item.Trans.Matrix, new Vector3(tri.a), 1).Point;
                Point3 b = MyMat.Mul44x41(item.Trans.Matrix, new Vector3(tri.b), 1).Point;
                Point3 c = MyMat.Mul44x41(item.Trans.Matrix, new Vector3(tri.c), 1).Point;
                //Point3 a =tri.a;
                //Point3 b = tri.b;
                //Point3 c =tri.c;

                min = new Point3();
                min.X = Math.Min(a.X, Math.Min(b.X, c.X));
                min.Y = Math.Min(a.Y, Math.Min(b.Y, c.Y));
                min.Z = Math.Max(a.Z, Math.Max(b.Z, c.Z));

                max = new Point3();
                max.X = Math.Max(a.X, Math.Max(b.X, c.X));
                max.Y = Math.Max(a.Y, Math.Max(b.Y, c.Y));
                max.Z = Math.Min(a.Z, Math.Min(b.Z, c.Z));
            }
            //min = MyMat.Mul44x41(item.Trans.Matrix, new Vector3(min), 1).Point;
            //max = MyMat.Mul44x41(item.Trans.Matrix, new Vector3(max), 1).Point;
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

            area = 2.0 * (size.X * size.Y + size.X * (size.Z * -1) + size.X * (size.Z * -1));

        }

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
                double ty1 = (min.Y - ray.Start.Y) / ray.Direction.Point.Y;
                double ty2 = (max.Y - ray.Start.Y) / ray.Direction.Point.Y;

                tmin = Math.Max(tmin, Math.Min(ty1, ty2));
                tmax = Math.Min(tmax, Math.Max(ty1, ty2));
            }

            if (ray.Direction.Point.Z != 0.0)
            {
                double tz1 = (min.Z - ray.Start.Z) / ray.Direction.Point.Z;
                double tz2 = (max.Z - ray.Start.Z) / ray.Direction.Point.Z;

                tmin = Math.Max(tmin, Math.Min(tz1, tz2));
                tmax = Math.Min(tmax, Math.Max(tz1, tz2));
            }

            return tmax >= tmin;

        }

    }
}
