using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Point3
    {

        public Point3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Point3()
            : this(0, 0, 0)
        {
        }

        public Point3(float[] parameter)
            : this(parameter[0], parameter[1], parameter[2])
        {
        }

        private float x, y, z;

        public float Z
        {
            get { return z; }
            set { z = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float X
        {
            get { return x; }
            set { x = value; }
        }


        public static Point3 ZERO { get { return new Point3(0, 0, 0); } }

        public static Point3 operator +(Point3 a, Point3 b)
        {
            return new Point3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Point3 operator -(Point3 a, Point3 b)
        {
            return a + (b * -1f);
        }

        public static Point3 operator *(Point3 a, float scale)
        {
            return new Point3(a.X * scale, a.Y * scale, a.Z * scale);
        }
        public static Point3 operator *(Point3 a, int scale)
        {
            return new Point3(a.X * scale, a.Y * scale, a.Z * scale);
        }

        public void ShowInformation()
        {
            Console.WriteLine(" Point:" + X.ToString("#.0000") + " " + Y.ToString("#.0000") + " " + Z.ToString("#.0000"));
        }
    }
}
