using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Matrix
    {


        float[,] value;
        public Matrix(int row, int col)
        {
            RowNumber = row;
            ColNumber = col;
            value = new float[RowNumber, ColNumber];
        }

        public Matrix(int row, int col, float[] val)
            : this(row, col)
        {
            SetValue(val);
        }

        public void SetValue(float[] value)
        {
            if (value.Length == this.value.Length)
                for (int row = 0; row < RowNumber; row++)
                {
                    for (int col = 0; col < ColNumber; col++)
                    {
                        this.value[row, col] = value[row * RowNumber + col];
                    }
                }
        }

        public void SetValue(int row, int col, float value)
        {
            this.value[row, col] = value;
        }



        public float[,] GetValue() { return value; }
        public float GetValue(int column, int row)
        {
            return value[column, row];
        }

        public Matrix GetRow(int row)
        {
            Matrix result = new Matrix(1, ColNumber);
            for (int i = 0; i < ColNumber; i++)
            {
                result.SetValue(0, i, this.value[row, i]);
            }
            return result;
        }

        public Matrix GetCol(int col)
        {
            Matrix result = new Matrix(RowNumber, 1);
            for (int i = 0; i < RowNumber; i++)
            {
                result.SetValue(i, 0, this.value[i, col]);
            }
            return result;
        }




        public int RowNumber
        {
            get;
            set;
        }


        public int ColNumber
        {
            get;
            set;
        }

        public static bool IsSameSize(Matrix a, Matrix b)
        {
            if ((a.ColNumber == b.ColNumber) && (a.RowNumber == b.RowNumber))
                return true;
            else return false;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            Matrix result = new Matrix(a.ColNumber, a.RowNumber);

            for (int row = 0; row < result.RowNumber; row++)
            {
                for (int col = 0; col < result.ColNumber; col++)
                {
                    float val = a.GetValue(row, col) + b.GetValue(row, col);
                    result.SetValue(row, col, val);
                }
            }
            return result;
        }

        public static Matrix operator *(Matrix a, float b)
        {
            Matrix result = new Matrix(a.ColNumber, a.RowNumber);

            for (int row = 0; row < result.RowNumber; row++)
                for (int col = 0; col < result.ColNumber; col++)
                {
                    float val = a.GetValue(row, col) * b;
                    result.SetValue(row, col, val);
                }
            return result;
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            return a + (b * -1);
        }


        public Matrix I
        {
            get
            {
                Matrix result = new Matrix(ColNumber, RowNumber);

                for (int row = 0; row < result.RowNumber; row++)

                    for (int col = 0; col < result.ColNumber; col++)
                    {
                        float val = (row == col) ? 1 : 0;
                        result.SetValue(row, col, val);
                    }
                return result;
            }

        }

        public Matrix Inverse4X4()
        {
            float s0 = value[0, 0] * value[1, 1] - value[1, 0] * value[0, 1];
            float s1 = value[0, 0] * value[1, 2] - value[1, 0] * value[0, 2];
            float s2 = value[0, 0] * value[1, 3] - value[1, 0] * value[0, 3];
            float s3 = value[0, 1] * value[1, 2] - value[1, 1] * value[0, 2];
            float s4 = value[0, 1] * value[1, 3] - value[1, 1] * value[0, 3];
            float s5 = value[0, 2] * value[1, 3] - value[1, 2] * value[0, 3];

            float c5 = value[2, 2] * value[3, 3] - value[3, 2] * value[2, 3];
            float c4 = value[2, 1] * value[3, 3] - value[3, 1] * value[2, 3];
            float c3 = value[2, 1] * value[3, 2] - value[3, 1] * value[2, 2];
            float c2 = value[2, 0] * value[3, 3] - value[3, 0] * value[2, 3];
            float c1 = value[2, 0] * value[3, 2] - value[3, 0] * value[2, 2];
            float c0 = value[2, 0] * value[3, 1] - value[3, 0] * value[2, 1];

            //Should check for 0 determinant
            float invdet = 1f / (s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0);

            Matrix result = new Matrix(4, 4);

            result.SetValue(0, 0, (value[1, 1] * c5 - value[1, 2] * c4 + value[1, 3] * c3) * invdet);
            result.SetValue(0, 1, (-value[0, 1] * c5 + value[0, 2] * c4 - value[0, 3] * c3) * invdet);
            result.SetValue(0, 2, (value[3, 1] * s5 - value[3, 2] * s4 + value[3, 3] * s3) * invdet);
            result.SetValue(0, 3, (-value[2, 1] * s5 + value[2, 2] * s4 - value[2, 3] * s3) * invdet);

            result.SetValue(1, 0, (-value[1, 0] * c5 + value[1, 2] * c2 - value[1, 3] * c1) * invdet);
            result.SetValue(1, 1, (value[0, 0] * c5 - value[0, 2] * c2 + value[0, 3] * c1) * invdet);
            result.SetValue(1, 2, (-value[3, 0] * s5 + value[3, 2] * s2 - value[3, 3] * s1) * invdet);
            result.SetValue(1, 3, (value[2, 0] * s5 - value[2, 2] * s2 + value[2, 3] * s1) * invdet);

            result.SetValue(2, 0, (value[1, 0] * c4 - value[1, 1] * c2 + value[1, 3] * c0) * invdet);
            result.SetValue(2, 1, (-value[0, 0] * c4 + value[0, 1] * c2 - value[0, 3] * c0) * invdet);
            result.SetValue(2, 2, (value[3, 0] * s4 - value[3, 1] * s2 + value[3, 3] * s0) * invdet);
            result.SetValue(2, 3, (-value[2, 0] * s4 + value[2, 1] * s2 - value[2, 3] * s0) * invdet);

            result.SetValue(3, 0, (-value[1, 0] * c3 + value[1, 1] * c1 - value[1, 2] * c0) * invdet);
            result.SetValue(3, 1, (value[0, 0] * c3 - value[0, 1] * c1 + value[0, 2] * c0) * invdet);
            result.SetValue(3, 2, (-value[3, 0] * s3 + value[3, 1] * s1 - value[3, 2] * s0) * invdet);
            result.SetValue(3, 3, (value[2, 0] * s3 - value[2, 1] * s1 + value[2, 2] * s0) * invdet);

            return result;

        }

        public static Vector3 Mult44x41(Matrix matrix, Vector3 vector)
        {

            float[,] val = matrix.GetValue();
            float x = vector.Value.X;
            float y = vector.Value.Y;
            float z = vector.Value.Z;

            float newX = val[0, 0] * x + val[0, 1] * y + val[0, 2] * z + val[0, 3] * 1;
            float newY = val[1, 0] * x + val[1, 1] * y + val[1, 2] * z + val[1, 3] * 1;
            float newZ = val[2, 0] * x + val[2, 1] * y + val[2, 2] * z + val[2, 3] * 1;

            return new Vector3(newX, newY, newZ);
        }



    }
}
