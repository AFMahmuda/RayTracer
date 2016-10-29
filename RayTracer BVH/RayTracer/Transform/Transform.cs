using RayTracer.Common;
using System;


namespace RayTracer.Transformation
{
    [Serializable]
    public class Transform
    {

        MyMatrix matrix;

        public MyMatrix Matrix
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
                    Console.Write(Matrix.Inverse.GetValue(row, col) + "\t");
                Console.WriteLine();
            }

            Console.WriteLine("=====================================================");
        }

        public Transform Clone()
        {
            return (Transform)this.MemberwiseClone();
        }
    }
}
