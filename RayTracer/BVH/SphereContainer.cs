using RayTracer.Common;
using RayTracer.Shape;
using RayTracer.Tracer;
using System;

namespace RayTracer.BVH
{
    [Serializable]
    public class SphereContainer : Container
    {

        public Point3 c; //center
        public float r; //radius

        public SphereContainer(Geometry item)
        {
            Type = TYPE.SPHERE;
            geo = item;
            if (item.GetType() == typeof(Sphere))
            {

                c = ((Sphere)item).c;
                r = ((Sphere)item).r;

            }
            else //if (item.GetType() == typeof(Triangle))
            {
                Triangle tri = (Triangle)item;
                Vec3 ab = new Vec3(tri.a, tri.b);
                Vec3 bc = new Vec3(tri.b, tri.c);
                Vec3 ac = new Vec3(tri.a, tri.c);

                float d = 2 * ((ab * ab) * (ac * ac) - (ab * ac) * (ab * ac));
                Point3 reference = tri.a;
                float s = ((ab * ab) * (ac * ac) - (ac * ac) * (ab * ac)) / d;
                float t = ((ac * ac) * (ab * ab) - (ab * ab) * (ab * ac)) / d;
                if (s <= 0)
                {
                    c = (tri.a + tri.c) * .5f;
                }
                else if (t <= 0)
                {
                    c = (tri.a + tri.b) * .5f;
                }
                else if (s + t > 1)
                {
                    c = (tri.b + tri.c) * .5f;
                    reference = tri.b;
                }
                else c = tri.a + (tri.b - tri.a) * s + (tri.c - tri.a) * t;
                r = (float)Math.Sqrt(new Vec3(reference, tri.c) * new Vec3(reference, tri.c));
            }

            c = Matrix.Mul44x41(item.Trans.Matrix, new Vec3(c), 1);
            r = Matrix.Mul44x41(item.Trans.Matrix, new Vec3(r, 0, 0), 0).Magnitude;
        }

        public SphereContainer(SphereContainer a, SphereContainer b)
        {
            Type = TYPE.SPHERE;
            childs[0]=a;
            childs[1]=b;

            Vec3 aToB = new Vec3(a.c, b.c);
            float aToBLength = aToB.Magnitude;

            if (aToB.Magnitude == 0)
            {
                r = Math.Max(a.r, b.r);
                c = a.c;
            }

            else if (aToBLength + a.r + b.r < a.r * 2 ||
                aToB.Magnitude + a.r + b.r < b.r * 2)
            {
                r = Math.Max(a.r, b.r);
                c = (a.r > b.r) ? a.c : b.c;
            }

            else
            {
                r = (a.r + b.r + aToB.Magnitude) * .5f;
                c = a.c + (aToB.Normalize() * (r - a.r));
            }

            area = 4f * (float)Math.PI * (float)Math.Pow(r, 2);           
        }

        public override bool IsIntersecting(Ray ray)
        {

            Vec3 rayToSphere = new Vec3(ray.Start, this.c);

            float a = ray.Direction * ray.Direction;
            float b = -2 * (rayToSphere * ray.Direction);
            float c = (rayToSphere * rayToSphere) - (r * r);
            float dd = (b * b) - (4 * a * c);

            return (dd > 0);
        }
    }
}
