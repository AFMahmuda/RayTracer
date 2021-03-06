﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer.Common
{
    [Serializable]
    public class Point3
    {

        public Point3(Double x, Double y, Double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Point3()
            : this(0, 0, 0)
        {
        }

        public Point3(Double[] parameter)
            : this(parameter[0], parameter[1], parameter[2])
        {
        }

        private Double x, y, z;

        public Double Z
        {
            get { return z; }
            set { z = value; }
        }

        public Double Y
        {
            get { return y; }
            set { y = value; }
        }

        public Double X
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

        public static Point3 operator *(Point3 a, Double scale)
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
