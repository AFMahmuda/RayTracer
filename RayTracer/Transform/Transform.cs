using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Transform
    {
         
        Matrix matrix;

        public Matrix Matrix
        {
            get { return matrix; }
            set { matrix = value; }
        }


        internal void ShowInformation()
        {
            Console.WriteLine("Transform Information======================================");
            Console.WriteLine("value");
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                    Console.Write(Matrix.GetValue(row, col) + "\t");
                Console.WriteLine();
            }
            Console.WriteLine("inverse");
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++) 
                    Console.Write(Matrix.Inverse4X4().GetValue(row, col) + "\t");
                Console.WriteLine();
            }

            Console.WriteLine("=====================================================");
        }

        public Transform Clone()
        {
            return (Transform) this.MemberwiseClone();
        }
    }
}
