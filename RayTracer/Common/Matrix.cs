using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer.Common
{
    [Serializable]
    public class Mattrix
    {

        int rowNumber;
        int colNumber;
        float[,] values;

        public Mattrix(int row, int col)
        {
            rowNumber = row;
            colNumber = col;
            values = new float[rowNumber, colNumber];
            haveInverse = false;
            haveIdentity = false;

        }

        public Mattrix(int row, int col, float[] val)
            : this(row, col)
        {
            SetValue(val);
        }

        public void SetValue(float[] value)
        {
            if (value.Length == this.values.Length)
                for (int row = 0; row < rowNumber; row++)
                    for (int col = 0; col < colNumber; col++)
                        this.values[row, col] = value[row * rowNumber + col];
            haveInverse = false;
        }

        public void SetValue(int row, int col, float value)
        {
            this.values[row, col] = value;
            haveInverse = false;
        }



        public float[,] GetValue() { return values; }
        public float GetValue(int column, int row)
        {
            return values[column, row];
        }

        public Mattrix GetRow(int row)
        {
            Mattrix result = new Mattrix(1, colNumber);
            for (int i = 0; i < colNumber; i++)
                result.SetValue(0, i, this.values[row, i]);
            return result;
        }

        public Mattrix GetCol(int col)
        {
            Mattrix result = new Mattrix(rowNumber, 1);
            for (int i = 0; i < rowNumber; i++)
                result.SetValue(i, 0, this.values[i, col]);
            return result;
        }

        public static Mattrix operator *(Mattrix a, float b)
        {
            Mattrix result = new Mattrix(a.colNumber, a.rowNumber);

            for (int row = 0; row < result.rowNumber; row++)
                for (int col = 0; col < result.colNumber; col++)
                {
                    float val = a.GetValue(row, col) * b;
                    result.SetValue(row, col, val);
                }
            return result;
        }


        private bool haveIdentity;
        private Mattrix identity;
        public Mattrix I
        {
            get
            {
                if (!haveIdentity)
                {
                    identity = CreateIdentity();
                    haveIdentity = true;
                }
                return identity;
            }
        }
        private Mattrix CreateIdentity()
        {
            Mattrix result = new Mattrix(colNumber, rowNumber);
            for (int row = 0; row < result.rowNumber; row++)
                for (int col = 0; col < result.colNumber; col++)
                    result.SetValue(row, col, (row == col) ? 1 : 0);
            return result;
        }



        bool haveInverse;
        Mattrix inverse;
        public Mattrix Inverse
        {
            get
            {
                if (!haveInverse)
                {
                    inverse = CreateInverse();
                    haveInverse = true;
                }
                return inverse;
            }
        }

        Mattrix CreateInverse()
        {
            float s0 = values[0, 0] * values[1, 1] - values[1, 0] * values[0, 1];
            float s1 = values[0, 0] * values[1, 2] - values[1, 0] * values[0, 2];
            float s2 = values[0, 0] * values[1, 3] - values[1, 0] * values[0, 3];
            float s3 = values[0, 1] * values[1, 2] - values[1, 1] * values[0, 2];
            float s4 = values[0, 1] * values[1, 3] - values[1, 1] * values[0, 3];
            float s5 = values[0, 2] * values[1, 3] - values[1, 2] * values[0, 3];

            float c5 = values[2, 2] * values[3, 3] - values[3, 2] * values[2, 3];
            float c4 = values[2, 1] * values[3, 3] - values[3, 1] * values[2, 3];
            float c3 = values[2, 1] * values[3, 2] - values[3, 1] * values[2, 2];
            float c2 = values[2, 0] * values[3, 3] - values[3, 0] * values[2, 3];
            float c1 = values[2, 0] * values[3, 2] - values[3, 0] * values[2, 2];
            float c0 = values[2, 0] * values[3, 1] - values[3, 0] * values[2, 1];

            float invdet = 1f / (s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0);

            Mattrix result = new Mattrix(4, 4);

            result.SetValue(0, 0, (values[1, 1] * c5 - values[1, 2] * c4 + values[1, 3] * c3) * invdet);
            result.SetValue(0, 1, (-values[0, 1] * c5 + values[0, 2] * c4 - values[0, 3] * c3) * invdet);
            result.SetValue(0, 2, (values[3, 1] * s5 - values[3, 2] * s4 + values[3, 3] * s3) * invdet);
            result.SetValue(0, 3, (-values[2, 1] * s5 + values[2, 2] * s4 - values[2, 3] * s3) * invdet);

            result.SetValue(1, 0, (-values[1, 0] * c5 + values[1, 2] * c2 - values[1, 3] * c1) * invdet);
            result.SetValue(1, 1, (values[0, 0] * c5 - values[0, 2] * c2 + values[0, 3] * c1) * invdet);
            result.SetValue(1, 2, (-values[3, 0] * s5 + values[3, 2] * s2 - values[3, 3] * s1) * invdet);
            result.SetValue(1, 3, (values[2, 0] * s5 - values[2, 2] * s2 + values[2, 3] * s1) * invdet);

            result.SetValue(2, 0, (values[1, 0] * c4 - values[1, 1] * c2 + values[1, 3] * c0) * invdet);
            result.SetValue(2, 1, (-values[0, 0] * c4 + values[0, 1] * c2 - values[0, 3] * c0) * invdet);
            result.SetValue(2, 2, (values[3, 0] * s4 - values[3, 1] * s2 + values[3, 3] * s0) * invdet);
            result.SetValue(2, 3, (-values[2, 0] * s4 + values[2, 1] * s2 - values[2, 3] * s0) * invdet);

            result.SetValue(3, 0, (-values[1, 0] * c3 + values[1, 1] * c1 - values[1, 2] * c0) * invdet);
            result.SetValue(3, 1, (values[0, 0] * c3 - values[0, 1] * c1 + values[0, 2] * c0) * invdet);
            result.SetValue(3, 2, (-values[3, 0] * s3 + values[3, 1] * s1 - values[3, 2] * s0) * invdet);
            result.SetValue(3, 3, (values[2, 0] * s3 - values[2, 1] * s1 + values[2, 2] * s0) * invdet);

            return result;
        }

        public static Vec3 Mul44x41(Mattrix matrix, Vec3 vector, int homogeneousValue)
        {

            float[,] val = matrix.GetValue();
            float x = vector.Point.X;
            float y = vector.Point.Y;
            float z = vector.Point.Z;

            float newX = val[0, 0] * x + val[0, 1] * y + val[0, 2] * z + val[0, 3] * homogeneousValue;
            float newY = val[1, 0] * x + val[1, 1] * y + val[1, 2] * z + val[1, 3] * homogeneousValue;
            float newZ = val[2, 0] * x + val[2, 1] * y + val[2, 2] * z + val[2, 3] * homogeneousValue;

            Vec3 result = new Vec3(newX, newY, newZ);

            return result;
        }

        public static Mattrix Mul44x44(Mattrix matA, Mattrix matB)
        {
            Mattrix res = new Mattrix(4, 4);

            for (int col = 0; col < 4; col++)
            {
                Mattrix mat41 = Mul44x41(matA, matB.GetCol(col));
                for (int row = 0; row < 4; row++)
                    res.SetValue(row, col, mat41.GetValue(row, 0));
            }

            return res;
        }


        public static Mattrix Mul44x41(Mattrix mat44, Mattrix mat41)
        {
            Mattrix res = new Mattrix(4, 1);

            for (int row = 0; row < 4; row++)
            {
                float val = 0;
                for (int col = 0; col < 4; col++)
                    val += mat44.GetValue(row, col) * mat41.GetValue(col, 0);
                res.SetValue(row, 0, val);
            }

            return res;
        }

    }
}
