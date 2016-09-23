using RayTracer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer.Transformation
{
    [Serializable]
    public class Rotation : Transform
    {
        public Rotation()
            : this(Point3.ZERO, 0f)
        { }

        public Rotation(Double[] values)
            : this(new Point3(values[0], values[1], values[2]), values[3])
        { }

        public Rotation(Point3 values, Double angle)
        {
            Matrix = new MyMatrix(4, 4);

            Double rad = (angle) * Math.PI / 180.0;

            Double x = values.X * rad;
            double cx = Math.Cos(x);
            double sx = Math.Sin(x);
            Double y = values.Y * rad;
            double cy = Math.Cos(y);
            double sy = Math.Sin(y);
            Double z = values.Z * rad;
            double cz = Math.Cos(z);
            double sz = Math.Sin(z);

            Matrix.SetValue(0, 0, cy * cz);
            Matrix.SetValue(0, 1, cz * sx * sy - cx * sz);
            Matrix.SetValue(0, 2, cx * cz * sy + sx * sz);

            Matrix.SetValue(1, 0, cy * sz);
            Matrix.SetValue(1, 1, cx * cz + sx * sy * sz);
            Matrix.SetValue(1, 2, -cz * sx + cx * sy * sz);

            Matrix.SetValue(2, 0, -sy);
            Matrix.SetValue(2, 1, cy * sx);
            Matrix.SetValue(2, 2, cx * cy);

            Matrix.SetValue(3, 3, 1f);
        }
    }
}
