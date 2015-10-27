using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
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
            Matrix = new Matrix(4, 4);
            Matrix = Matrix.I;
            float rad = (angle) * (float)Math.PI / 180;

            float x = values.X;
            float y = values.Y;
            float z = values.Z;
            float c = (float)Math.Cos(rad);
            float s = (float)Math.Sin(rad);

            Matrix.SetValue(0, 0, x * x * (1 - c) + c);
            Matrix.SetValue(0, 1, x * y * (1 - c) - z * s);
            Matrix.SetValue(0, 2, x * z * (1 - c) + y * s);

            Matrix.SetValue(1, 0, y * x * (1 - c) + z * s);
            Matrix.SetValue(1, 1, y * y * (1 - c) + c);
            Matrix.SetValue(1, 2, y * z * (1 - c) - x * s);

            Matrix.SetValue(2, 0, z * x * (1 - c) - y * s);
            Matrix.SetValue(2, 1, z * y * (1 - c) + x * s);
            Matrix.SetValue(2, 2, z * z * (1 - c) + c);

            //float x = values.X * rad;
            //float y = values.Y * rad;
            //float z = values.Z * rad;

            //float cosX = (float)Math.Cos(x);
            //float sinX = (float)Math.Sin(x);

            //float cosY = (float)Math.Cos(y);
            //float sinY = (float)Math.Sin(y);

            //float cosZ = (float)Math.Cos(y);
            //float sinZ = (float)Math.Sin(y);

            //Matrix rotateX = new Matrix(4, 4);
            //Matrix rotateY = new Matrix(4, 4);
            //Matrix rotateZ = new Matrix(4, 4);

            //rotateX = rotateX.I;
            //rotateX.SetValue(1, 1, cosX);
            //rotateX.SetValue(1, 2, -sinX);
            //rotateX.SetValue(2, 1, sinX);
            //rotateX.SetValue(2, 2, cosX);

            //rotateY = rotateY.I;
            //rotateY.SetValue(0, 0, cosY);
            //rotateY.SetValue(0, 2, sinY);
            //rotateY.SetValue(2, 0, -sinY);
            //rotateY.SetValue(2, 2, cosY);

            //rotateZ = rotateZ.I;
            //rotateZ.SetValue(0, 0, cosZ);
            //rotateZ.SetValue(0, 1, -sinZ);
            //rotateZ.SetValue(1, 0, sinZ);
            //rotateZ.SetValue(1, 1, cosZ);


            //Matrix = Matrix.Mult44x44(Matrix.Mult44x44(rotateZ, rotateY), rotateX);




        }
    }
}
