﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer.Common
{
    [Serializable]
    public class MyMat
    {

        int rowNumber;
        int colNumber;
        float[,] value;

        public MyMat(int row, int col)
        {
            rowNumber = row;
            colNumber = col;
            value = new float[rowNumber, colNumber];
            haveInverse = false;
            haveIdentity = false;

        }

        public MyMat(int row, int col, float[] val)
            : this(row, col)
        {
            SetValue(val);
        }

        public void SetValue(float[] value)
        {
            if (value.Length == this.value.Length)
                for (int row = 0; row < rowNumber; row++)
                    for (int col = 0; col < colNumber; col++)
                        this.value[row, col] = value[row * rowNumber + col];
            haveInverse = false;
        }

        public void SetValue(int row, int col, float value)
        {
            this.value[row, col] = value;
            haveInverse = false;
        }



        public float[,] GetValue() { return value; }
        public float GetValue(int column, int row)
        {
            return value[column, row];
        }

        public MyMat GetRow(int row)
        {
            MyMat result = new MyMat(1, colNumber);
            for (int i = 0; i < colNumber; i++)
                result.SetValue(0, i, this.value[row, i]);
            return result;
        }

        public MyMat GetCol(int col)
        {
            MyMat result = new MyMat(rowNumber, 1);
            for (int i = 0; i < rowNumber; i++)
                result.SetValue(i, 0, this.value[i, col]);
            return result;
        }

        public static MyMat operator *(MyMat a, float b)
        {
            MyMat result = new MyMat(a.colNumber, a.rowNumber);

            for (int row = 0; row < result.rowNumber; row++)
                for (int col = 0; col < result.colNumber; col++)
                {
                    float val = a.GetValue(row, col) * b;
                    result.SetValue(row, col, val);
                }
            return result;
        }


        private bool haveIdentity;
        private MyMat identity;
        public MyMat I
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
        private MyMat CreateIdentity()
        {
            MyMat result = new MyMat(colNumber, rowNumber);
            for (int row = 0; row < result.rowNumber; row++)
                for (int col = 0; col < result.colNumber; col++)
                    result.SetValue(row, col, (row == col) ? 1 : 0);
            return result;
        }



        bool haveInverse;
        MyMat inverse;
        public MyMat Inverse
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

        MyMat CreateInverse()
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

            float invdet = 1f / (s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0);

            MyMat result = new MyMat(4, 4);

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

        public static Vector3 Mul44x41(MyMat matrix, Vector3 vector, int homogeneousValue)
        {

            float[,] val = matrix.GetValue();
            float x = vector.Point.X;
            float y = vector.Point.Y;
            float z = vector.Point.Z;

            float newX = val[0, 0] * x + val[0, 1] * y + val[0, 2] * z + val[0, 3] * homogeneousValue;
            float newY = val[1, 0] * x + val[1, 1] * y + val[1, 2] * z + val[1, 3] * homogeneousValue;
            float newZ = val[2, 0] * x + val[2, 1] * y + val[2, 2] * z + val[2, 3] * homogeneousValue;

            Vector3 result = new Vector3(newX, newY, newZ);

            return result;
        }

        public static MyMat Mult44x44(MyMat matA, MyMat matB)
        {
            MyMat res = new MyMat(4, 4);

            for (int col = 0; col < 4; col++)
            {
                MyMat mat41 = Mult44x41(matA, matB.GetCol(col));
                for (int row = 0; row < 4; row++)
                    res.SetValue(row, col, mat41.GetValue(row, 0));
            }

            return res;
        }


        public static MyMat Mult44x41(MyMat mat44, MyMat mat41)
        {
            MyMat res = new MyMat(4, 1);

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
