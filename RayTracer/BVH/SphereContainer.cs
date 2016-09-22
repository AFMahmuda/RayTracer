using RayTracer.Common;
using RayTracer.Shape;
using RayTracer.Tracer;
using RayTracer.Transformation;
using System;

namespace RayTracer.BVH
{
    public class SphereContainer : Container
    {

        public Point3 center;
        public double radius;

        public SphereContainer(Geometry item)
        {
            geo = item;
            if (item.GetType() == typeof(Sphere))
            {
                center = ((Sphere)item).center;
                radius = ((Sphere)item).radius;

                center = MyMatrix.Mult44x41(item.Trans.Matrix, new Vector3(center), 1).Point;
                radius = MyMatrix.Mult44x41(item.Trans.Matrix, new Vector3(radius, 0, 0), 0).Magnitude;

            }
        }

        public SphereContainer(SphereContainer a, SphereContainer b)
        {

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
            //Console.Write("c1 : pos : " + a.center.X + " , " + a.center.Y + " , " + a.center.Z + "rad : " + a.radius + " \n" +
            //                "c2 : pos : " + b.center.X + " , " + b.center.Y + " , " + b.center.Z + "rad : " + b.radius + " \n" +
            //                "new: pos : " + center.X + " , " + center.Y + " , " + center.Z + "rad : " + radius + " \n"
            //    );
        }

        public override bool IsIntersecting(Ray ray)
        {

            Vector3 rayToSphere = new Vector3(ray.Start, center);

            //sphere is behind cam
            if (rayToSphere * ray.Direction <= 0)
                return false;

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
