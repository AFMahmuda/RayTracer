using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Triangle : Geometry
    {

        public Triangle(Point3 a, Point3 b, Point3 c)
        {
            A = a;
            B = b;
            C = c;
        }


        public Point3 A
        { get; set; }

        public Point3 B
        { get; set; }

        public Point3 C
        { get; set; }

        public override bool CheckIntersection(Ray ray)
        {
            Vector3 norm = Vector3.Cross(new Vector3(C - A), new Vector3(B - A));
            norm /= norm.Magnitude;
            if (ray.Direction * norm == 0)
                return false;

            float t = (new Vector3(A) * norm - new Vector3(ray.Start) * norm) / (ray.Direction * norm);
            
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

        public bool IsInsideTriangle(Point3 point)
        {
            Vector3 ab = new Vector3(B - A);
            Vector3 ac = new Vector3(C - A);
            Vector3 ap = new Vector3(point - A);

            float dot00 = ab * ab;
            float dot01 = ab * ac;
            float dot02 = ab * ap;
            float dot11 = ac * ac;
            float dot12 = ac * ap;


            float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
            float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            return (u >= 0) && (v >= 0) && (u + v <= 1);

        }



        public override Vector3 GetNormal(Point3 point)
        {
            Vector3 norm = Vector3.Cross(new Vector3(C - A), new Vector3(B - A)).Normalize();
            norm = Matrix.Mult44x41(Transform.Matrix.Inverse4X4(), norm, 1);
            return norm;
        }


       
    }
}
