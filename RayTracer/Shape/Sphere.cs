﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
        [Serializable]
    public class Sphere : Geometry
    {

        Point3 center;
        float radius;

        public Sphere()
            : this(Point3.ZERO, 0f)
        { }

        public Sphere(float[] values)
            : this(new Point3(values[0], values[1], values[2]), values[3])
        { }
        public Sphere(Point3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }



        public override bool IsIntersecting(Ray ray)
        {
            Vector3 camToSphere = new Vector3(ray.Start, center);

            //false if sphere is behind ray
            if (camToSphere * ray.Direction <= 0)
                return false;

            float a = ray.Direction * ray.Direction;
            float b = -2 * (camToSphere * ray.Direction);
            float c = (camToSphere * camToSphere) - (radius * radius);
            float dd = (b * b) - (4 * a * c);

            if (dd > 0)
            {
                float res1 = (-b + (float)Math.Sqrt(dd)) / 2f * a;
                float res2 = (-b - (float)Math.Sqrt(dd)) / 2f * a;
                float distance;

                // if both results are negative, then the sphere is behind our ray, 
                // but we already checked that.
                //if (res1 < 0 && res2 < 0)
                //    return false;
                //else
                if (res1 * res2 < 0)
                    distance = Math.Max(res1, res2);
                else
                    distance = Math.Min(res1, res2);

                if (ray.IsSmallerThanCurrent(distance, Transform))
                {
                    ray.IntersectDistance = MyMatrix.Mult44x41(Transform.Matrix, ray.Direction * distance, 0).Magnitude;
                    return true;
                }
            }
            return false;
        }

        public override Vector3 GetNormal(Point3 point)
        {
            point = MyMatrix.Mult44x41(Transform.Matrix.Inverse, new Vector3(point), 1).Value;
            Vector3 norm = new Vector3(center, point).Normalize();
//            norm = Matrix.Mult44x41(Transform.Matrix.Inverse, norm, 0).Normalize();
            return norm;
        }
    }
}
