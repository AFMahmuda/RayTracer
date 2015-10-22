using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Scaling : Transform
    {

        public Scaling()
            : this(Point3.ZERO)
        { }

        public Scaling(float[] values)
            : this(new Point3(values))
        { }

        public Scaling(Point3 values)
        {
            Matrix = new Matrix(3,3);
        }
    }
}
