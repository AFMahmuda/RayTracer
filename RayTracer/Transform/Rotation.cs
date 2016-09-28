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

        public Rotation(float[] values)
            : this(new Point3(values[0], values[1], values[2]), values[3])
        { }

        public Rotation(Point3 values, float angle)
        {
            Matrix = new MyMat(4, 4);

            float rad = (angle) * (float) Math.PI / 180.0f;

            float x = values.X * rad;
            float cx = (float) Math.Cos(x);
            float sx = (float) Math.Sin(x);
            float y = values.Y * rad;
            float cy = (float) Math.Cos(y);
            float sy = (float) Math.Sin(y);
            float z = values.Z * rad;
            float cz = (float) Math.Cos(z);
            float sz = (float) Math.Sin(z);

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
