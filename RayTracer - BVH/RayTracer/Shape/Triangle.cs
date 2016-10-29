using RayTracer.Common;
using RayTracer.Tracer;
using RayTracer.Transformation;
using System;

namespace RayTracer.Shape
{
    [Serializable]
    public class Triangle : Geometry
    {
        public readonly Point3 a;
        public readonly Point3 b;
        public readonly Point3 c;


        //precalculated vals
        private Vec3 localNorm;

        private Vec3 ab;
        private Vec3 ac;

        private float dot_ab_ab;
        private float dot_ab_ac;
        private float dot_ac_ac;
        private float dot_ab_ap;
        private float dot_ac_ap;

        private float invDenom;

        public Triangle(Point3 a, Point3 b, Point3 c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            Trans = new Translation();
            hasMorton = false;
            PreCalculate();
        }

        void PreCalculate()
        {

            //for IsInsideTriangle
            ab = new Vec3(a, b);
            ac = new Vec3(a, c);

            //for IsIntersect
            localNorm = Vec3.Cross(ac, ab).Normalize();

            dot_ab_ab = ab * ab;
            dot_ab_ac = ab * ac;
            dot_ac_ac = ac * ac;

            invDenom = 1.0f / (dot_ab_ab * dot_ac_ac - dot_ab_ac * dot_ab_ac);
        }
        private Object _lock = new Object();
        public override bool IsIntersecting(Ray ray)
        {
            lock (_lock)
            {
                //parallel -> return false
                if (ray.Direction * localNorm == 0)
                    return false;
                /*
                relative to ray direction
                */
                float distanceToPlane = (
                     (new Vec3(a) * localNorm) -
                     (new Vec3(ray.Start) * localNorm))
                    / (ray.Direction * localNorm);
                /*
                dist < 0 = behind cam
                */
                if (distanceToPlane > 0)
                    if (ray.IsSmallerThanCurrent(distanceToPlane, Trans))
                        if (IsInsideTriangle(ray.Start + (ray.Direction * distanceToPlane)))
                        {
                            ray.IntersectDistance = Matrix.Mul44x41(Trans.Matrix, ray.Direction * distanceToPlane, 0).Magnitude;
                            return true;
                        }
                return false;
            }

        }


        bool IsInsideTriangle(Point3 point)
        {
            Vec3 ap = new Vec3(point - a);

            dot_ab_ap = ab * ap;
            dot_ac_ap = ac * ap;

            float u = (dot_ac_ac * dot_ab_ap - dot_ab_ac * dot_ac_ap) * invDenom;
            float v = (dot_ab_ab * dot_ac_ap - dot_ab_ac * dot_ab_ap) * invDenom;

            return (u >= 0) && (v >= 0) && (u + v <= 1);
        }


        public override Vec3 GetNormal(Point3 point)
        {
            return Matrix.Mul44x41(Trans.Matrix.Inverse, localNorm, 0).Normalize();
        }

        public override void UpdatePos()
        {
            Vec3 temp = new Vec3(a + b + c) * (.33f);
            pos = Matrix.Mul44x41(Trans.Matrix, temp, 1);
            pos.X = pos.X / 100f + .5f;
            pos.Y = pos.Y / 100f + .5f;
            pos.Z = pos.Z / 100f + .5f;
        }
    }
}
