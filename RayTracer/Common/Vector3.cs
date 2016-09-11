using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    [Serializable]
    public class Vector3
    {

        public Vector3(Double x, Double y, Double z)
        {
            Point = new Point3(x, y, z);
        }

        public Vector3(Point3 point)
        {
            //point is a reference! we only need value.
            Point = point * 1;
        }

        public Vector3(Point3 start, Point3 end)
        {
            this.Point = end - start;
        }

        public static Vector3 UP { get { return new Vector3(0, 1, 0); } }
        public static Vector3 RIGHT { get { return new Vector3(1, 0, 0); } }
        public static Vector3 DOWN { get { return UP * -1; } }
        public static Vector3 LEFT { get { return RIGHT * -1; } }



        public Point3 Point
        { get; set; }

        public Double Magnitude
        {
            get { return Math.Sqrt((Point.X * Point.X) + (Point.Y * Point.Y) + (Point.Z * Point.Z)); }
        }

        public Vector3 Normalize()
        {
            return this / Magnitude;
        }


        public static Double operator *(Vector3 a, Vector3 b)
        {
            Point3 newA = a.Point;
            Point3 newB = b.Point;
            Double result = ((newA.X * newB.X) + (newA.Y * newB.Y) + (newA.Z * newB.Z));

            return result;
        }

        public static Vector3 operator *(Vector3 vector, Double scalar)
        {
            Vector3 result = new Vector3(vector.Point);
            result.Point.X *= scalar;
            result.Point.Y *= scalar;
            result.Point.Z *= scalar;
            return result;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {


            Vector3 newA = a * 1;
            Vector3 newB = b * 1;
            Vector3 result = new Vector3(newA.Point + newB.Point);
            return result;

        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return a + (b * -1);
        }

        public static Vector3 operator /(Vector3 vector, Double scalar)
        {
            return vector * (1.0 / scalar);
        }

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            Point3 A = a.Point;
            Point3 B = b.Point;
            return new Vector3(
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
