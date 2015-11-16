using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
        [Serializable]
    public class Scaling : Transform
    {

        public Scaling()
            : this(new Point3(1,1,1))
        { }

        public Scaling(Double[] values)
            : this(new Point3(values))
        { }

        public Scaling(Point3 values)
        {
            Matrix = new MyMatrix(4, 4);

            Matrix.SetValue(0, 0, values.X);
            Matrix.SetValue(1, 1, values.Y);
            Matrix.SetValue(2, 2, values.Z);

            Matrix.SetValue(3, 3, 1f);
           // ShowInformation();
        }

       
    }
}
