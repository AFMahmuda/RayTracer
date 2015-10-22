using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Vector3
    {

        public Vector3(float x, float y, float z)
            : this(new Point3(x, y, z))
        { }

        public Vector3(Point3 point)
            : this(Point3.ZERO, point)
        { }

        public Vector3(Point3 start, Point3 end)
        {
            this.Start = start;
            this.End = end;
        }

        public static Vector3 UP { get { return new Vector3(0, 1, 0); } }
        public static Vector3 RIGHT { get { return new Vector3(1, 0, 0); } }
        public static Vector3 DOWN { get { return UP * -1; } }
        public static Vector3 LEFT { get { return RIGHT * -1; } }

        public void ShowInformation()
        {
            Console.WriteLine(" vector:");
            Console.WriteLine(" Start     = " + Start.X.ToString("#.0000") + " " + Start.Y.ToString("#.0000") + " " + Start.Z.ToString("#.0000"));
            Console.WriteLine(" End       = " + End.X.ToString("#.0000") + " " + End.Y.ToString("#.0000") + " " + End.Z.ToString("#.0000"));
            Console.WriteLine(" Magnitude = " + Magnitude.ToString("#.0000"));
        }
        public Point3 Start
        { get; set; }

        public Point3 End
        { get; set; }

        public float Magnitude
        {
            get { return Distance(Start, End); }
        }

        public static float Distance(Point3 start, Point3 end)
        {
            Point3 temp = end - start;
            return (float)Math.Sqrt((temp.X * temp.X) + (temp.Y * temp.Y) + (temp.Z * temp.Z));
        }

        public static float operator *(Vector3 a, Vector3 b)
        {
            Point3 newA = a.End - a.Start;
            Point3 newB = a.End - a.Start;
            float result = (newA.X * newB.X + newA.Y * newB.Y + newA.Z * newB.Z);

            return result;
        }

        public static Vector3 operator *(Vector3 vector, float scalar)
        {
            Vector3 result = new Vector3(vector.End - vector.Start);
            result.End.X *= scalar;
            result.End.Y *= scalar;
            result.End.Z *= scalar;
            return result;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return Vector3.UP;//to be implemented
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return a + (b * -1);
        }

        public static Vector3 operator /(Vector3 vector, float scalar)
        {
            return vector * (1f / scalar);
        }

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            Point3 A = a.End - a.Start;
            Point3 B = b.End - b.Start;
            return new Vector3(
                A.Y * B.Z - A.Z * B.Y,
                (A.X * B.Z - A.Z * B.X) * -1,
                A.X * B.Y - A.Y * B.X
            );


        }

    }
}
