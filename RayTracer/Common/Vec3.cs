using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer.Common
{
    [Serializable]
    public class Vec3
    {

        public Vec3(float x, float y, float z)
        {
            Point = new Point3(x, y, z);
        }

        public Vec3(Point3 point)
        {
            //point is a reference! we only need value.
            Point = point * 1;
        }

        public Vec3(Point3 start, Point3 end)
        {
            this.Point = end - start;
        }

        public static Vec3 UP { get { return new Vec3(0, 1, 0); } }
        public static Vec3 RIGHT { get { return new Vec3(1, 0, 0); } }
        public static Vec3 DOWN { get { return UP * -1; } }
        public static Vec3 LEFT { get { return RIGHT * -1; } }



        public Point3 Point
        { get; set; }

        public float Magnitude
        {
            get { return (float)Math.Sqrt((Point.X * Point.X) + (Point.Y * Point.Y) + (Point.Z * Point.Z)); }
        }

        public Vec3 Normalize()
        {
            return this / Magnitude;
        }


        public static float operator *(Vec3 a, Vec3 b)
        {
            Point3 newA = a.Point;
            Point3 newB = b.Point;
            float result = ((newA.X * newB.X) + (newA.Y * newB.Y) + (newA.Z * newB.Z));

            return result;
        }

        public static Vec3 operator *(Vec3 vector, float scalar)
        {
            Vec3 result = new Vec3(vector.Point);
            result.Point.X *= scalar;
            result.Point.Y *= scalar;
            result.Point.Z *= scalar;
            return result;
        }

        public static Vec3 operator +(Vec3 a, Vec3 b)
        {


            Vec3 newA = a * 1;
            Vec3 newB = b * 1;
            Vec3 result = new Vec3(newA.Point + newB.Point);
            return result;

        }

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            return a + (b * -1);
        }

        public static Vec3 operator /(Vec3 vector, float scalar)
        {
            return vector * (1.0f / scalar);
        }

        public static Vec3 Cross(Vec3 a, Vec3 b)
        {
            Point3 A = a.Point;
            Point3 B = b.Point;
            return new Vec3(
                A.Y * B.Z - A.Z * B.Y,
                (A.X * B.Z - A.Z * B.X) * -1,
                A.X * B.Y - A.Y * B.X
            );
        }

        public void ShowInformation()
        {
            Console.WriteLine(" vector:");
            Console.WriteLine(" Value     = " + Point.X.ToString("#.0000") + " " + Point.Y.ToString("#.0000") + " " + Point.Z.ToString("#.0000"));
            Console.WriteLine(" Magnitude = " + Magnitude.ToString("#.0000"));
        }
    }
}
