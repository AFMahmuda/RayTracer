using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Triangle : Geometry
    {

        Point3 a;
        Point3 b;
        Point3 c;

        Vector3 localNorm;

        Vector3 ab;
        Vector3 ac;
        Vector3 ap;

        //dot products
        float dot_ab_ab;
        float dot_ab_ac;
        float dot_ac_ac;
        float dot_ab_ap;
        float dot_ac_ap;

        float invDenom;

        public Triangle(Point3 a, Point3 b, Point3 c)
        {
            this.a = a;
            this.b = b;
            this.c = c;

            PreCalculate();
        }

        void PreCalculate()
        {
            //for IsIntersecting
            localNorm = Vector3.Cross(new Vector3(b - a), new Vector3(c - a)).Normalize();


            //for IsInsideTriangle
            ab = new Vector3(b - a);
            ac = new Vector3(c - a);

            dot_ab_ab = ab * ab;
            dot_ab_ac = ab * ac;
            dot_ac_ac = ac * ac;

            invDenom = 1 / (dot_ab_ab * dot_ac_ac - dot_ab_ac * dot_ab_ac);
        }

        public override bool IsIntersecting(Ray ray)
        {
            if (ray.Direction * localNorm == 0)
                return false;

            float t = (new Vector3(a) * localNorm - new Vector3(ray.Start) * localNorm) / (ray.Direction * localNorm);

            if (t > 0 && ray.IsSmallerThanCurrent(t, Transform))
            {
                if (IsInsideTriangle(ray.Start + (ray.Direction * t).Value))
                {
                    ray.IntersectDistance = Matrix.Mult44x41(Transform.Matrix, ray.Direction * t, 0).Magnitude;
                    return true;
                }

            }
            return false;
        }



       

        bool IsInsideTriangle(Point3 point)
        {
            ap = new Vector3(point - a);

            dot_ab_ap = ab * ap;
            dot_ac_ap = ac * ap;

            float u = (dot_ac_ac * dot_ab_ap - dot_ab_ac * dot_ac_ap) * invDenom;
            float v = (dot_ab_ab * dot_ac_ap - dot_ab_ac * dot_ab_ap) * invDenom;

            return (u >= 0) && (v >= 0) && (u + v <= 1);
        }



        public override Vector3 GetNormal(Point3 point)
        {
            Vector3 norm = Matrix.Mult44x41(Transform.Matrix.Inverse, localNorm, 1).Normalize();
            return norm;
        }



    }
}
