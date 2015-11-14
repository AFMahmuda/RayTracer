using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    [Serializable]
    public class Camera
    {

        public Camera()
            : this(Point3.ZERO, Point3.ZERO, Vector3.UP, 30f)
        {

        }

        public Camera(float[] parameter)
            : this(
            new Point3(parameter[0], parameter[1], parameter[2]),
            new Point3(parameter[3], parameter[4], parameter[5]),
            new Vector3(parameter[6], parameter[7], parameter[8]),
            parameter[9]
            )
        {

        }

        public Point3 CameraViewPosition()
        {
            return (U * Position.X + V * Position.Y + W * Position.Z).Value;
        }


        public Camera(Point3 position, Point3 lookAt, Vector3 up, float fov)
        {
            this.Position = position;
            this.LookAt = lookAt;
            this.Up = up;
            FieldOfView = fov;

            W = (new Vector3(LookAt - Position)).Normalize();
            U = Vector3.Cross(Up, W).Normalize();
            V = Vector3.Cross(W, U);

            //ShowInformation();
        }


        public void ShowInformation()
        {

            Console.WriteLine("Camera Information ================================");
            Console.Write("Pos    : ");
            Position.ShowInformation();
            Console.Write("LookAt : ");
            LookAt.ShowInformation();
            Console.WriteLine("Up");
            Up.ShowInformation();

            Console.Write("W : ");
            W.ShowInformation();
            Console.Write("U : ");
            U.ShowInformation();
            Console.Write("V : ");
            V.ShowInformation();
            Console.WriteLine("===================================================");
        }


        Vector3 w;
        Vector3 u;
        Vector3 v;



        public Vector3 W
        {
            get { return w; }
            set { w = value; }
        }
        public Vector3 U
        {
            get { return u; }
            set { u = value; }
        }


        public Vector3 V
        {
            get { return v; }
            set { v = value; }
        }
        private Point3 position;
        private Point3 lookAt;
        private Vector3 up;
        private float fieldOfView;

        public float FieldOfView
        {
            get { return fieldOfView; }
            set
            {
                if (value < 0)
                    FieldOfView = 0;
                else if (value > 360)
                    FieldOfView = 360;
                else
                    fieldOfView = value;
            }
        }

        public Vector3 Up
        {
            get { return up; }
            set { up = value; }
        }

        public Point3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Point3 LookAt
        {
            get { return lookAt; }
            set { lookAt = value; }
        }

    }
}
