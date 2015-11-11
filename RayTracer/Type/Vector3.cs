using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
        [Serializable]
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
            this.Value = end - start;
        }

        public static Vector3 UP { get { return new Vector3(0, 1, 0); } }
        public static Vector3 RIGHT { get { return new Vector3(1, 0, 0); } }
        public static Vector3 DOWN { get { return UP * -1; } }
        public static Vector3 LEFT { get { return RIGHT * -1; } }



        public Point3 Value
        { get; set; }

        public float Magnitude
        {
            get { return (float)Math.Sqrt((Value.X * Value.X) + (Value.Y * Value.Y) + (Value.Z * Value.Z)); }
        }

        public Vector3 Normalize()
        {
            return this / Magnitude;
        }


        public static float operator *(Vector3 a, Vector3 b)
        {
            Point3 newA = a.Value;
            Point3 newB = b.Value;
            float result = ((newA.X * newB.X) + (newA.Y * newB.Y) + (newA.Z * newB.Z));

            return result;
        }

        public static Vector3 operator *(Vector3 vector, float scalar)
        {
            Vector3 result = new Vector3(vector.Value);
            result.Value.X *= scalar;
            result.Value.Y *= scalar;
            result.Value.Z *= scalar;
            return result;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {


            Vector3 newA = a * 1;
            Vector3 newB = b * 1;
            Vector3 result = new Vector3(newA.Value + newB.Value);
            return result;

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
            Point3 A = a.Value;
            Point3 B = b.Value;
            return new Vector3(
                A.Y * B.Z - A.Z * B.Y,
                (A.X * B.Z - A.Z * B.X) * -1,
                A.X * B.Y - A.Y * B.X
            );
        }

        public void ShowInformation()
        {
            Console.WriteLine(" vector:");
            Console.WriteLine(" Value     = " + Value.X.ToString("#.0000") + " " + Value.Y.ToString("#.0000") + " " + Value.Z.ToString("#.0000"));
            Console.WriteLine(" Magnitude = " + Magnitude.ToString("#.0000"));
        }
    }
}
