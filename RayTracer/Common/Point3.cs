using System;

namespace RayTracer.Common
{
    [Serializable]
    public class Point3
    {
        public Point3(float x, float y, float z)
        {
            vals = new float[] { x, y, z };
        }

        public Point3()
            : this(0, 0, 0)
        { }

        public Point3(float[] parameter)
            : this(parameter[0], parameter[1], parameter[2])
        { }

        public Point3(Point3 point)
            : this(point[0], point[1], point[2])
        { }

        public static Point3 ZERO { get { return new Point3(0, 0, 0); } }


        protected float[] vals;
        public float this[int i]
        {
            get
            {
                return vals[i];
            }
            set
            {
                vals[i] = value;
            }
        }
        public float Z
        {
            get { return vals[2]; }
            set { vals[2] = value; }
        }

        public float Y
        {
            get { return vals[1]; }
            set { vals[1] = value; }
        }

        public float X
        {
            get { return vals[0]; }
            set { vals[0] = value; }
        }

        public static Point3 operator *(Point3 point, float scalar)
        {
            return new Point3(point.X * scalar, point.Y * scalar, point.Z * scalar);
        }

        public static Point3 operator +(Point3 a, Point3 b)
        {
            return new Point3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Point3 operator -(Point3 a, Point3 b)
        {
            return a + (b * -1f);
        }


    }
}
