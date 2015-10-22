using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class Scene
    {
        private Size size = new Size();

        public Size Size
        {
            get { return size; }
            set { size = value; }
        }
        private int maxDepth = 5;
        private Camera camera;
        private ViewPlane viewPlane;
        private LinkedList<Geometry> geometries = new LinkedList<Geometry>();
        private LinkedList<Transform> transforms = new LinkedList<Transform>();
        private LinkedList<Light> lights = new LinkedList<Light>();
        private String outputFilename;
//        private LinkedList<Point3> vertices
         
        public Scene()
        {
            camera = new Camera();
            viewPlane = new ViewPlane(1, 1, 30);
        }

        public Scene(String scenefile)
        {
            StreamReader filereader = new StreamReader(scenefile);
            String command;
            while ((command = filereader.ReadLine()) != null)
                ExecuteCommand(command);
            filereader.Close();
        }


        public void ExecuteCommand(String fullcommand)
        {
            if (fullcommand.Contains('#'))
                return;
            fullcommand = fullcommand.Trim();
            String[] words = fullcommand.Split(' ');
            String command = words[0];

            if (command.Equals("output"))
            {
                OutputFilename = words[1] + ".bmp";
                return;
            }

            float[] parameter = new float[words.Length - 1];

            for (int i = 0; i < parameter.Length; i++)
                parameter[i] = float.Parse(words[i + 1]);

            switch (command)
            {
                case "size":
                    Size = new Size((int)parameter[0], (int)parameter[1]);
                    break;
                case "camera":

                    Point3 lookFrom = new Point3(parameter[0],parameter[1],parameter[2]);
                    Point3 lookAt = new Point3(parameter[3],parameter[4],parameter[5]);
                    Vector3 up = new Vector3(parameter[6],parameter[7],parameter[8]);
                    float fov = parameter[9]; 
                    Camera = new Camera(lookFrom,lookAt,up,fov);
                    break;
                case "maxdepth":
                    MaxDepth = (int) parameter[0];
                    break;
                case "sphere":
                    Point3 center = new Point3(parameter[0], parameter[1], parameter[2]);
                    float radius = parameter[3];
                    Sphere sphere = new Sphere(center,radius);
                    ApplyTransform(sphere);
                    Geometries.AddFirst(sphere);
                    break;
                //case "tri":
                //    Point3 a = new Point3(parameter[0]);
                //    Point3 b = new Point3(parameter[1]);
                //    Point3 c = new Point3(parameter[2]);
                //    Triangle tri = new Triangle(a, b, c);
                //    Geometries.AddFirst(tri);
                //    break;
                //case "maxverts":

//                    break;

                default:
                    break;
            }
        }


        public Camera Camera
        {
            get { return camera; }
            set { camera = value; }
        }


        public ViewPlane ViewPlane
        {
            get { return viewPlane; }
            set { viewPlane = value; }
        }


        public LinkedList<Geometry> Geometries
        {
            get { return geometries; }
            set { geometries = value; }
        }

        public LinkedList<Transform> Transforms
        {
            get { return transforms; }
            set { transforms = value; }
        }

        public LinkedList<Light> Lights
        {
            get { return lights; }
            set { lights = value; }
        }

        public Material Material
        {
            get;
            set;
        }

        public int MaxDepth
        {
            get { return maxDepth; }
            set { maxDepth = value; }
        }


        public String OutputFilename
        {
            get { return outputFilename; }
            set { outputFilename = value; }
        }

        private void ApplyTransform(Geometry shape)
        { 
            
        }

    }
}
