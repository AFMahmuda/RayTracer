using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Rotation : Transform
    {
        public Rotation()
            : this(Point3.ZERO,0f)
        { }

        public Rotation(float[] values)
            : this(new Point3(values[0],values[1],values[2]),values[3])
        { }

        public Rotation(Point3 values, float angle)
        {
            Matrix = new Matrix(3,3);
        }
    }
}
