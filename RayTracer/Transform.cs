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



        public Transform(TYPE type, Point3 transformvalue, float angle = 1f)
        {
            matrix = new Matrix(3, 3);
            switch (type)
            {
                case TYPE.TRANSLATION:
                    Translate(transformvalue);
                    break;
                case TYPE.ROTATION:
                    Rotate(transformvalue, angle);
                    break;
                case TYPE.SCALING:
                    Scale(transformvalue);
                    break;
                default:
                    break;
            }
        }



        public enum TYPE { TRANSLATION, ROTATION, SCALING };
        public TYPE Type
        { set; get; }

        private void Translate(Point3 translation)
        {
            throw new System.NotImplementedException();
        }

        private void Rotate(Point3 pivot, float angle)
        {
            throw new System.NotImplementedException();
        }

        private void Scale(Point3 scale)
        {
            throw new System.NotImplementedException();
        }


    }
}
