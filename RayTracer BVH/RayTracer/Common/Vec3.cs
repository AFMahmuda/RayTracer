using System;

namespace RayTracer.Common
{
    [Serializable]
    public class Vec3 : Point3
    {
        public Vec3(float x, float y, float z)
        {
            vals = new float[] { x, y, z};
        }

        public Vec3(Vec3 vector)
            :this(vector[0], vector[0], vector[0])
        {
        }
        public Vec3(Point3 point)
        : this(point[0], point[1], point[2])
        {
        }

        public Vec3(Point3 start, Point3 end)
            :this(end - start)
        {
            
        }

        public static Vec3 UP { get { return new Vec3(0, 1, 0); } }
        public static Vec3 RIGHT { get { return new Vec3(1, 0, 0); } }
        public static Vec3 DOWN { get { return (UP * -1); } }
        public static Vec3 LEFT { get { return (RIGHT * -1); } }


        public float Magnitude
        {
            get { return (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z)); }
        }

        public Vec3 Normalize()
        {
            return this / Magnitude;
        }


        public static float operator *(Vec3 a, Vec3 b)
        {
            Vec3 newA = a;
            Vec3 newB = b;
            float result = ((newA.X * newB.X) + (newA.Y * newB.Y) + (newA.Z * newB.Z));

            return result;
        }

      
        public static Vec3 operator +(Vec3 a, Vec3 b)
        {            
            Vec3 result = new Vec3(a[0] + b[0], a[1] + b[1], a[2] + b[2]);
            return result;

        }

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            return a + (b * -1);
        }
        public static Vec3 operator *(Vec3 vector, float scalar)
        {
            return new Vec3(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
        }
        public static Vec3 operator /(Vec3 vector, float scalar)
        {
            return (vector * (1.0f / scalar));
        }

        public static Vec3 Cross(Vec3 a, Vec3 b)
        {
            Vec3 A = a;
            Vec3 B = b;
            return new Vec3(
                A.Y * B.Z - A.Z * B.Y,
                (A.X * B.Z - A.Z * B.X) * -1,
                A.X * B.Y - A.Y * B.X
            );
        }
    }
}
