using RayTracer.Common;
using RayTracer.Shape;
using RayTracer.Tracer;
using RayTracer.Transformation;
using System;

namespace RayTracer.BVH
{
    [Serializable]
    public class SphereContainer : Container
    {

        public Point3 center;
        public double radius;

        public SphereContainer(Geometry item)
        {
            Type = TYPE.SPHERE;
            geo = item;
            if (item.GetType() == typeof(Sphere))
            {

                center = ((Sphere)item).center;
                radius = ((Sphere)item).radius;

            }
            else //if (item.GetType() == typeof(Triangle))
            {
                Triangle tri = (Triangle)item;
                Vector3 ab = new Vector3(tri.a, tri.b);
                Vector3 bc = new Vector3(tri.b, tri.c);
                Vector3 ac = new Vector3(tri.a, tri.c);

                double d = 2 * ((ab * ab) * (ac * ac) - (ab * ac) * (ab * ac));
                Point3 reference = tri.a;
                double s = ((ab * ab) * (ac * ac) - (ac * ac) * (ab * ac)) / d;
                double t = ((ac * ac) * (ab * ab) - (ab * ab) * (ab * ac)) / d;
                if (s <= 0)
                {
                    center = (tri.a + tri.c) * .5;
                }
                else if (t <= 0)
                {
                    center = (tri.a + tri.b) * .5;
                }
                else if (s + t > 1)
                {
                    center = (tri.b + tri.c) * .5;
                    reference = tri.b;
                }
                else center = tri.a + (tri.b - tri.a) * s + (tri.c - tri.a) * t;
                radius = Math.Sqrt(new Vector3(reference, tri.c) * new Vector3(reference, tri.c));
            }

            center = MyMat.Mul44x41(item.Trans.Matrix, new Vector3(center), 1).Point;
            radius = MyMat.Mul44x41(item.Trans.Matrix, new Vector3(radius, 0, 0), 0).Magnitude;
        }

        public SphereContainer(SphereContainer a, SphereContainer b)
        {
            Type = TYPE.SPHERE;
            childs.Add(a);
            childs.Add(b);

            Vector3 aToB = new Vector3(a.center, b.center);
            double aToBLength = aToB.Magnitude;

            if (aToB.Magnitude == 0)
            {
                radius = Math.Max(a.radius, b.radius);
                center = a.center;
            }

            else if (aToBLength + a.radius + b.radius < a.radius * 2 ||
                aToB.Magnitude + a.radius + b.radius < b.radius * 2)
            {
                radius = Math.Max(a.radius, b.radius);
                center = (a.radius > b.radius) ? a.center : b.center;
            }

            else
            {
                radius = (a.radius + b.radius + aToB.Magnitude) * .5;
                center = a.center + (aToB.Normalize() * (radius - a.radius)).Point;
            }

            area = 4 * Math.PI * Math.Pow(radius, 2);           
        }

        public override bool IsIntersecting(Ray ray)
        {

            Vector3 rayToSphere = new Vector3(ray.Start, center);

            //if (rayToSphere * ray.Direction <= 0)
            //    return false;

            Double a = ray.Direction * ray.Direction;
            Double b = -2 * (rayToSphere * ray.Direction);
            Double c = (rayToSphere * rayToSphere) - (radius * radius);
            Double dd = (b * b) - (4 * a * c);

            return (dd > 0);
        }

        internal void ShowInformation()
        {
            Console.WriteLine("pos : " + center.X + " , " + center.Y + " , " + center.Z + " rad : " + radius);
        }
    }
}
