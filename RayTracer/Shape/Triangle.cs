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
            if (t > 0 && t < ray.Intersection)
            {
                ray.Intersection = t;
                return IsInsideTriangle((ray.Direction*t).Value);
            }

            Console.WriteLine(t);

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

            // Compute barycentric coordinates
            float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
            float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            // Check if point is in triangle
            return (u >= 0) && (v >= 0) && (u + v <= 1);

        }


        public override Vector3 CalculateReflection(Ray ray)
        {
            throw new NotImplementedException();
        }

        public override void transformToCameraSpace(Vector3 U, Vector3 V, Vector3 W)
        {
            A = ((U * A.X) + (V * A.Y) + (W * A.Z)).Value;
            B = ((U * B.X) + (V * B.Y) + (W * B.Z)).Value;
            C = ((U * C.X) + (V * C.Y) + (W * C.Z)).Value;
        }
    }
}
