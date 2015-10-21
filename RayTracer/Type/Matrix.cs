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


        public Matrix I()
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
}
