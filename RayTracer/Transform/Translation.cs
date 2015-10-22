using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Translation : Transform
    {
        public Translation()
            : this(Point3.ZERO)
        { }

        public Translation(float[] values)
            : this(new Point3(values))
        { }

        public Translation(Point3 values)
        {
            Matrix = new Matrix(3, 3);
        }

    }
}
