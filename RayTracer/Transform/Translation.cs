using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RayTracer
{
        [Serializable]
    public class Translation : Transform
    {
        public Translation()
            : this(Point3.ZERO)
        { }

        public Translation(Double[] values)
            : this(new Point3(values))
        { }

        public Translation(Point3 values)
        {
            Matrix = new MyMatrix(4, 4);
            Matrix = Matrix.I;

            Matrix.SetValue(0, 3, values.X);
            Matrix.SetValue(1, 3, values.Y);
            Matrix.SetValue(2, 3, values.Z);

           // ShowInformation();
        }


    }
}
