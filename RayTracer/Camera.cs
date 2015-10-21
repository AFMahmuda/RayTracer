using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Camera
    {

        public Camera()
            : this(Point3.ZERO, Point3.ZERO, Vector3.UP, 0f)
        {

        }

        public Camera(Point3 position, Point3 lookAt, Vector3 up, float fov)
        {
            this.Position = position;
            this.LookAt = lookAt;
            this.Up = up;

            W = (new Vector3(Position,LookAt)) / new Vector3(Position,LookAt).Magnitude;
            U = Up * W / (Up * W).Magnitude;
            V = W * U;

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
