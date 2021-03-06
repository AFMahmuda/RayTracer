﻿using RayTracer.Common;
using RayTracer.Tracer;
using RayTracer.Transformation;
using System;

namespace RayTracer.Shape
{
    [Serializable]
    public class Sphere : Geometry
    {

        public Point3 c; //center
        public float r; //radius

        public Sphere()
            : this(Point3.ZERO, 0f)
        { }

        public Sphere(float[] values)
            : this(new Point3(values[0], values[1], values[2]), values[3])
        { }
        public Sphere(Point3 center, float radius)
        {

            c = center;
            r = radius;
            Trans = new Scaling();
            hasMorton = false;
        }

        public override bool IsIntersecting(Ray r)
        {


            Vec3 rayToSphere = new Vec3(r.Start, this.c);

            //sphere is behind ray
            //if (rayToSphere * ray.Direction <= 0)
            //    return false;

            float a = r.Direction * r.Direction;
            float b = -2 * (rayToSphere * r.Direction);
            float c = (rayToSphere * rayToSphere) - (this.r * this.r);
            float dd = (b * b) - (4 * a * c);

            if (dd > 0)
            {
                float res1 = (-b + (float)Math.Sqrt(dd)) / (2.0f * a);
                float res2 = (-b - (float)Math.Sqrt(dd)) / (2.0f * a);
                float distance;

                // if both results are negative, then the sphere is behind our ray, 
                // but we already checked that.
                if (res1 < 0 && res2 < 0)
                    return false;

                distance = (res1 * res2 < 0) ? Math.Max(res1, res2) : Math.Min(res1, res2);

                if (r.IsSmallerThanCurrent(distance, Trans))
                {
                    r.IntersectDistance = Matrix.Mul44x41(Trans.Matrix, r.Direction * distance, 0).Magnitude;
                    return true;
                }
            }
            return false;


        }

        public override Vec3 GetNormal(Point3 point)
        {
            point = Matrix.Mul44x41(Trans.Matrix.Inverse, new Vec3(point), 1);
            Vec3 norm = new Vec3(c, point).Normalize();
            return norm;
        }

        public override void UpdatePos()
        {
            pos = Matrix.Mul44x41(Trans.Matrix, new Vec3(c), 1);

            // todo : think a way to normalize position with ??? range
            pos.X = pos.X / 100f + .5f;
            pos.Y = pos.Y / 100f + .5f;
            pos.Z = pos.Z / 100f + .5f;
        }
    }
}
