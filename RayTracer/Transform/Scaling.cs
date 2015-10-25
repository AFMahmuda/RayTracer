using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Scaling : Transform
    {

        public Scaling()
            : this(new Point3(1,1,1))
        { }

        public Scaling(float[] values)
            : this(new Point3(values))
        { }

        public Scaling(Point3 values)
        {
            Matrix = new Matrix(4, 4);
            Matrix = Matrix.I;

            Matrix.SetValue(0, 0, values.X);
            Matrix.SetValue(1, 1, values.Y);
            Matrix.SetValue(2, 2, values.Z);


            ShowInformation();
        }

       
    }
}
