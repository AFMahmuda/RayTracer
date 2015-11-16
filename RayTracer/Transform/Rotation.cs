using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
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

            Double rad = (angle) * Math.PI / 180;

            Double x = values.X;
            Double y = values.Y;
            Double z = values.Z;
            Double c = Math.Cos(rad);
            Double s = Math.Sin(rad);

            Matrix.SetValue(0, 0, x * x * (1 - c) + c);
            Matrix.SetValue(0, 1, x * y * (1 - c) - z * s);
            Matrix.SetValue(0, 2, x * z * (1 - c) + y * s);

            Matrix.SetValue(1, 0, y * x * (1 - c) + z * s);
            Matrix.SetValue(1, 1, y * y * (1 - c) + c);
            Matrix.SetValue(1, 2, y * z * (1 - c) - x * s);

            Matrix.SetValue(2, 0, z * x * (1 - c) - y * s);
            Matrix.SetValue(2, 1, z * y * (1 - c) + x * s);
            Matrix.SetValue(2, 2, z * z * (1 - c) + c);

            Matrix.SetValue(3, 3, 1f);
        }
    }
}
