using RayTracer.Common;
using System;

namespace RayTracer.Tracer
{
    [Serializable]
    public class Camera
    {
        public static Camera Instance;

        public Camera()
            : this(Point3.ZERO, Point3.ZERO, Vec3.UP, 30f)
        { }

        public Camera(float[] parameter)
            : this(
            new Point3(parameter[0], parameter[1], parameter[2]), //position point
            new Point3(parameter[3], parameter[4], parameter[5]), //look at point
            new Vec3(parameter[6], parameter[7], parameter[8]), // up dir
            parameter[9] // field of view
            )
        { }



        public Camera(Point3 position, Point3 lookAt, Vec3 up, float fov)
        {
            Position = position;
            LookAt = lookAt;
            Up = up;
            FieldOfView = fov;

            W = (new Vec3(LookAt - Position)).Normalize();
            U = Vec3.Cross(Up, W).Normalize();
            V = Vec3.Cross(W, U);

            Instance = this;
        }

        public Point3 CameraViewPosition()
        {
            return (U * Position.X + V * Position.Y + W * Position.Z).Point;
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

        public Vec3 U
        { get; set; }


        public Vec3 V
        { get; set; }


        public Vec3 W
        { get; set; }


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
        public Vec3 Up
        { get; set; }


        public Point3 Position
        { get; set; }


        public Point3 LookAt
        { get; set; }

    }
}
