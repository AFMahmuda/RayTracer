using RayTracer.Common;
using RayTracer.Tracer;
using RayTracer.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer.Shape
{
    [Serializable]
    public class Sphere : Geometry
    {

        public Point3 center;
        public float radius;

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
            Trans = new Scaling();
            hasMorton = false;
        }

        public override bool IsIntersecting(Ray ray)
        {


            Vector3 rayToSphere = new Vector3(ray.Start, center);

            //sphere is behind ray
            if (rayToSphere * ray.Direction <= 0)
                return false;

            float a = ray.Direction * ray.Direction;
            float b = -2 * (rayToSphere * ray.Direction);
            float c = (rayToSphere * rayToSphere) - (radius * radius);
            float dd = (b * b) - (4 * a * c);

            if (dd > 0)
            {
                float res1 = (-b + (float)Math.Sqrt(dd)) / (2.0f * a);
                float res2 = (-b - (float)Math.Sqrt(dd)) / (2.0f * a);
                float distance;

                // if both results are negative, then the sphere is behind our ray, 
                // but we already checked that.
                //if (res1 < 0 && res2 < 0)
                //    return false;
                //else
                distance = (res1 * res2 < 0) ? Math.Max(res1, res2) : Math.Min(res1, res2);
                if (ray.IsSmallerThanCurrent(distance, Trans))
                {
                    ray.IntersectDistance = MyMat.Mul44x41(Trans.Matrix, ray.Direction * distance, 0).Magnitude;
                    return true;
                }
            }
            return false;


        }

        public override Vector3 GetNormal(Point3 point)
        {
            point = MyMat.Mul44x41(Trans.Matrix.Inverse, new Vector3(point), 1).Point;
            Vector3 norm = new Vector3(center, point).Normalize();
            return norm;
        }

        public override void UpdatePos()
        {
            pos = MyMat.Mul44x41(Trans.Matrix, new Vector3(center), 1).Point;

            // todo : think a way to normalize position with ??? range
            pos.X /= (10f) + .5f;
            pos.Y /= (10f) + .5f;
            pos.Z /= (10f) + .5f;
        }
    }
}
