﻿using System;
using RayTracer.Common;
using RayTracer.Shape;
using RayTracer.Tracer;

namespace RayTracer.BVH
{
    [Serializable]
    public class BoxContainer : Container
    {

        private Point3 min = new Point3(float.MaxValue, float.MaxValue, float.MaxValue);
        private Point3 max = new Point3(float.MinValue, float.MinValue, float.MinValue);

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
                min.Z -= sphere.radius;


                max = Utils.DeepClone(sphere.center);
                max.X += sphere.radius;
                max.Y += sphere.radius;
                max.Z += sphere.radius;

                for (int i = 0; i < 3; i++)
                {
                    min.Vals[i] -= sphere.radius;
                    max.Vals[i] += sphere.radius;
                }

                Point3[] p = new Point3[8];

                p[0] = (new Point3(min.X, min.Y, min.Z));
                p[1] = (new Point3(min.X, min.Y, max.Z));
                p[2] = (new Point3(min.X, max.Y, min.Z));
                p[3] = (new Point3(min.X, max.Y, max.Z));
                p[4] = (new Point3(max.X, min.Y, min.Z));
                p[5] = (new Point3(max.X, min.Y, max.Z));
                p[6] = (new Point3(max.X, max.Y, min.Z));
                p[7] = (new Point3(max.X, max.Y, max.Z));

                for (int i = 0; i < p.Length; i++)
                    p[i] = Mattrix.Mul44x41(item.Trans.Matrix, new Vec3(p[i]), 1).Point;

                SetMinMax(p);

            }

            else //if (item.GetType() == typeof(Triangle))
            {
                Triangle tri = (Triangle)item;

                Point3 a = Mattrix.Mul44x41(item.Trans.Matrix, new Vec3(tri.a), 1).Point;
                Point3 b = Mattrix.Mul44x41(item.Trans.Matrix, new Vec3(tri.b), 1).Point;
                Point3 c = Mattrix.Mul44x41(item.Trans.Matrix, new Vec3(tri.c), 1).Point;

                SetMinMax(new Point3[] { a, b, c });
            }
        }

        public BoxContainer(BoxContainer a, BoxContainer b)
        {
            Type = TYPE.BOX;
            childs[0] = a;
            childs[1] = b;


            for (int i = 0; i < 3; i++)
            {
                min.Vals[i] = Math.Min(a.min.Vals[i], b.min.Vals[i]);
                max.Vals[i] = Math.Max(a.max.Vals[i], b.max.Vals[i]);
            }
            Point3 size = max - min;

            area = 2f * (size.X * size.Y + size.X * (size.Z) + size.Y * (size.Z));

        }

        public override bool IsIntersecting(Ray ray)
        {
            float tmin = float.MinValue, tmax = float.MaxValue;

            for (int i = 0; i < 3; i++)
            {
                if (ray.Direction.Point.Vals[i] != 0f)
                {
                    float invTemp = 1f / ray.Direction.Point.Vals[i];
                    float tx1 = (min.Vals[i] - ray.Start.Vals[i]) * invTemp;
                    float tx2 = (max.Vals[i] - ray.Start.Vals[i]) * invTemp;

                    tmin = Math.Max(tmin, Math.Min(tx1, tx2));
                    tmax = Math.Min(tmax, Math.Max(tx1, tx2));
                }
            }
            
            return tmax >= tmin;

        }

        void SetMinMax(Point3[] points)
        {
            min = new Point3(float.MaxValue, float.MaxValue, float.MaxValue);
            max = new Point3(float.MinValue, float.MinValue, float.MinValue);
            for (int i = 0; i < points.Length; i++)
                for (int j = 0; j < 3; j++)
                {
                    min.Vals[j] = Math.Min(min.Vals[j], points[i].Vals[j]);
                    max.Vals[j] = Math.Max(max.Vals[j], points[i].Vals[j]);
                }
        }
    }
}
