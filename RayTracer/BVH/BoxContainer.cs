using System;
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
                min = Utils.DeepClone(sphere.c);
                min.X -= sphere.r;
                min.Y -= sphere.r;
                min.Z -= sphere.r;


                max = Utils.DeepClone(sphere.c);
                max.X += sphere.r;
                max.Y += sphere.r;
                max.Z += sphere.r;

                for (int i = 0; i < 3; i++)
                {
                    min[i] -= sphere.r;
                    max[i] += sphere.r;
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
                    p[i] = Matrix.Mul44x41(item.Trans.Matrix, new Vec3(p[i]), 1);

                SetMinMax(p);

            }

            else //if (item.GetType() == typeof(Triangle))
            {
                Triangle tri = (Triangle)item;

                Point3 a = Matrix.Mul44x41(item.Trans.Matrix, new Vec3(tri.a), 1);
                Point3 b = Matrix.Mul44x41(item.Trans.Matrix, new Vec3(tri.b), 1);
                Point3 c = Matrix.Mul44x41(item.Trans.Matrix, new Vec3(tri.c), 1);

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
                min[i] = Math.Min(a.min[i], b.min[i]);
                max[i] = Math.Max(a.max[i], b.max[i]);
            }
            Point3 size = max - min;

            area = 2f * (size.X * size.Y + size.X * (size.Z) + size.Y * (size.Z));

        }

        public override bool IsIntersecting(Ray ray)
        {
            float tmin = float.MinValue, tmax = float.MaxValue;

            for (int i = 0; i < 3; i++)
            {
                if (ray.Direction[i] != 0f)
                {
                    float invTemp = 1f / ray.Direction[i];
                    float tx1 = (min[i] - ray.Start[i]) * invTemp;
                    float tx2 = (max[i] - ray.Start[i]) * invTemp;

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
                    min[j] = Math.Min(min[j], points[i][j]);
                    max[j] = Math.Max(max[j], points[i][j]);
                }
        }
    }
}
