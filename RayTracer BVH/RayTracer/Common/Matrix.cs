using System;

namespace RayTracer.Common
{
    [Serializable]
    public class Matrix
    {

        int rowNumber;
        int colNumber;
        float[,] vals;

        public float this[int indexI, int indexJ]
        {
            get
            {
                return vals[indexI, indexJ];
            }

            set
            {
                vals[indexI, indexJ] = value;
            }
        }

        public Matrix(int row, int col)
        {
            rowNumber = row;
            colNumber = col;
            vals = new float[rowNumber, colNumber];
            haveInverse = false;
            haveIdentity = false;

        }

        public Matrix(int row, int col, float[] val)
            : this(row, col)
        {
            SetValue(val);
        }

        public void SetValue(float[] value)
        {
            if (value.Length == this.vals.Length)
                for (int row = 0; row < rowNumber; row++)
                    for (int col = 0; col < colNumber; col++)
                        this.vals[row, col] = value[row * rowNumber + col];
            haveInverse = false;
        }

        public void SetValue(int row, int col, float value)
        {
            this.vals[row, col] = value;
            haveInverse = false;
        }



        public float[,] GetValue() { return vals; }


        public Matrix GetRow(int row)
        {
            Matrix result = new Matrix(1, colNumber);
            for (int i = 0; i < colNumber; i++)
                result.SetValue(0, i, this[row, i]);
            return result;
        }

        public Matrix GetCol(int col)
        {
            Matrix result = new Matrix(rowNumber, 1);
            for (int i = 0; i < rowNumber; i++)
                result.SetValue(i, 0, this[i, col]);
            return result;
        }

        public static Matrix operator *(Matrix a, float b)
        {
            Matrix result = new Matrix(a.rowNumber, a.colNumber);

            for (int row = 0; row < result.rowNumber; row++)
                for (int col = 0; col < result.colNumber; col++)
                {
                    float val = a[row, col] * b;
                    result.SetValue(row, col, val);
                }
            return result;
        }


        private bool haveIdentity;
        private Matrix identity;
        public Matrix I
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
        private Matrix CreateIdentity()
        {
            Matrix result = new Matrix(colNumber, rowNumber);
            for (int row = 0; row < result.rowNumber; row++)
                for (int col = 0; col < result.colNumber; col++)
                    result[row, col] = (row == col) ? 1 : 0;
            return result;
        }



        bool haveInverse;
        Matrix inverse;
        public Matrix Inverse
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

        Matrix CreateInverse()
        {
            float s0 = vals[0, 0] * vals[1, 1] - vals[1, 0] * vals[0, 1];
            float s1 = vals[0, 0] * vals[1, 2] - vals[1, 0] * vals[0, 2];
            float s2 = vals[0, 0] * vals[1, 3] - vals[1, 0] * vals[0, 3];
            float s3 = vals[0, 1] * vals[1, 2] - vals[1, 1] * vals[0, 2];
            float s4 = vals[0, 1] * vals[1, 3] - vals[1, 1] * vals[0, 3];
            float s5 = vals[0, 2] * vals[1, 3] - vals[1, 2] * vals[0, 3];

            float c5 = vals[2, 2] * vals[3, 3] - vals[3, 2] * vals[2, 3];
            float c4 = vals[2, 1] * vals[3, 3] - vals[3, 1] * vals[2, 3];
            float c3 = vals[2, 1] * vals[3, 2] - vals[3, 1] * vals[2, 2];
            float c2 = vals[2, 0] * vals[3, 3] - vals[3, 0] * vals[2, 3];
            float c1 = vals[2, 0] * vals[3, 2] - vals[3, 0] * vals[2, 2];
            float c0 = vals[2, 0] * vals[3, 1] - vals[3, 0] * vals[2, 1];

            float invdet = 1f / (s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0);

            Matrix result = new Matrix(4, 4);

            result[0, 0] = (vals[1, 1] * c5 - vals[1, 2] * c4 + vals[1, 3] * c3) * invdet;
            result[0, 1] = (-vals[0, 1] * c5 + vals[0, 2] * c4 - vals[0, 3] * c3) * invdet;
            result[0, 2] = (vals[3, 1] * s5 - vals[3, 2] * s4 + vals[3, 3] * s3) * invdet;
            result[0, 3] = (-vals[2, 1] * s5 + vals[2, 2] * s4 - vals[2, 3] * s3) * invdet;

            result[1, 0] = (-vals[1, 0] * c5 + vals[1, 2] * c2 - vals[1, 3] * c1) * invdet;
            result[1, 1] = (vals[0, 0] * c5 - vals[0, 2] * c2 + vals[0, 3] * c1) * invdet;
            result[1, 2] = (-vals[3, 0] * s5 + vals[3, 2] * s2 - vals[3, 3] * s1) * invdet;
            result[1, 3] = (vals[2, 0] * s5 - vals[2, 2] * s2 + vals[2, 3] * s1) * invdet;

            result[2, 0] = (vals[1, 0] * c4 - vals[1, 1] * c2 + vals[1, 3] * c0) * invdet;
            result[2, 1] = (-vals[0, 0] * c4 + vals[0, 1] * c2 - vals[0, 3] * c0) * invdet;
            result[2, 2] = (vals[3, 0] * s4 - vals[3, 1] * s2 + vals[3, 3] * s0) * invdet;
            result[2, 3] = (-vals[2, 0] * s4 + vals[2, 1] * s2 - vals[2, 3] * s0) * invdet;

            result[3, 0] = (-vals[1, 0] * c3 + vals[1, 1] * c1 - vals[1, 2] * c0) * invdet;
            result[3, 1] = (vals[0, 0] * c3 - vals[0, 1] * c1 + vals[0, 2] * c0) * invdet;
            result[3, 2] = (-vals[3, 0] * s3 + vals[3, 1] * s1 - vals[3, 2] * s0) * invdet;
            result[3, 3] = (vals[2, 0] * s3 - vals[2, 1] * s1 + vals[2, 2] * s0) * invdet;

            return result;
        }

        public static Vec3 Mul44x41(Matrix matrix, Vec3 vector, int homogeneousValue)
        {

            Matrix m = matrix;
            float x = vector.X;
            float y = vector.Y;
            float z = vector.Z;

            float newX = m[0, 0] * x + m[0, 1] * y + m[0, 2] * z + m[0, 3] * homogeneousValue;
            float newY = m[1, 0] * x + m[1, 1] * y + m[1, 2] * z + m[1, 3] * homogeneousValue;
            float newZ = m[2, 0] * x + m[2, 1] * y + m[2, 2] * z + m[2, 3] * homogeneousValue;

            Vec3 result = new Vec3(newX, newY, newZ);

            return result;
        }

        public static Matrix Mul44x44(Matrix matA, Matrix matB)
        {
            Matrix res = new Matrix(4, 4);

            for (int col = 0; col < 4; col++)
            {
                Matrix mat41 = Mul44x41(matA, matB.GetCol(col));
                for (int row = 0; row < 4; row++)
                    res[row, col]= mat41[row, 0];
            }

            return res;
        }


        public static Matrix Mul44x41(Matrix mat44, Matrix mat41)
        {
            Matrix res = new Matrix(4, 1);

            for (int row = 0; row < 4; row++)
            {
                float val = 0;
                for (int col = 0; col < 4; col++)
                    val += mat44[row, col] * mat41[col, 0];
                res[row, 0] = val;
            }

            return res;
        }

    }
}
