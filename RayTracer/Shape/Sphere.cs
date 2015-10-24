﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Sphere : Geometry
    {

        public Sphere()
            : this(Point3.ZERO, 0f)
        { }

        public Sphere(float[] values)
            : this(new Point3(values[0], values[1], values[2]), values[3])
        { }
        public Sphere(Point3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        private Point3 center;
        private float radius;

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }


        public Point3 Center
        {
            get { return center; }
            set { center = value; }
        }

        public override bool CheckIntersection(Ray ray)
        {

            Vector3 camToSphere = new Vector3(ray.Start,Center);

            float a = ray.Direction * ray.Direction;
            float b = - 2 * (camToSphere * ray.Direction);
            float c = (camToSphere * camToSphere) - (Radius * Radius);
            float dd = (b * b) - (4 * a * c);
            //ray.Direction.ShowInformation();
            //camToSphere.ShowInformation();
            //Console.WriteLine(a + " " + b + " " + c + " " + dd);

            if (dd <= 0)
            {
                return false;
            }
            else if (dd > 0)
            {

                float t1 = (-b + (float)Math.Sqrt(dd)) / 2f * a;
                float t2 = (-b - (float)Math.Sqrt(dd)) / 2f * a;
//                Console.WriteLine(t1 + " " + t2);


                if (t1 < 0 && t2 < 0)
                    return false;
                else if (t1 < 0)
                {
                    if (t2 < ray.Intersection)
                    {
                        ray.Intersection = t2;
                        return true;
                    }
                    else return false;
                }
                else if (t2 < 0)
                {
                    if (t1 < ray.Intersection)
                    {
                        ray.Intersection = t1;
                        return true;
                    }
                    else return false;
                }

                else
                {
                    if (Math.Min(t1, t2) < ray.Intersection)
                    {
                        ray.Intersection = Math.Min(t1, t2);
                        return true;
                    }
                    else return false;
                }
            }
            return false;
        }

        public override Vector3 CalculateReflection(Ray ray)
        {
            throw new NotImplementedException();
        }

        public override void transformToCameraSpace(Vector3 U, Vector3 V, Vector3 W)
        {
            Center = ((U * Center.X) + (V * Center.Y) + (W * Center.Z)).Value;
        }


    }
}
