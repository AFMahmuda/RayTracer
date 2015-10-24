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
        private List<Point3> vertices = new List<Point3>();

        public Scene()
        {
            camera = new Camera();
            viewPlane = new ViewPlane(1, 1, camera);
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

            float[] param = new float[words.Length - 1];

            for (int i = 0; i < param.Length; i++)
                param[i] = float.Parse(words[i + 1]);

            switch (command)
            {
                case "size":
                    Size = new Size((int)param[0], (int)param[1]);
                    break;
                case "camera":
                    Camera = new Camera(param);
                    break;
                case "maxdepth":
                    MaxDepth = (int)param[0];
                    break;
                case "sphere":
                    Sphere sphere = new Sphere(param);
                    ApplyTransform(sphere);
                    sphere.transformToCameraSpace(Camera.U, Camera.V, Camera.W);
                    Geometries.AddLast(sphere);
                    break;
                case "tri":
                    Point3 a = vertices[(int)param[0]];
                    Point3 b = vertices[(int)param[1]];
                    Point3 c = vertices[(int)param[2]];

                    Triangle tri = new Triangle(a, b, c);
                    ApplyTransform(tri);
                    tri.transformToCameraSpace(Camera.U, Camera.V, Camera.W);
                    Geometries.AddLast(tri);
                    break;
                case "maxverts":
                    vertices = new List<Point3>((int)param[0]);
                    break;
                case "vertex":
                    vertices.Add(new Point3(param));
                    break;
                case "translate":
                    transforms.AddFirst(new Translation(param));
                    break;
                case "scale":
                    transforms.AddFirst(new Scaling(param));
                    break;
                case "rotate":
                    transforms.AddFirst(new Rotation(param));
                    break;




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
            foreach (var item in Transforms)
            {
                //TO DO : Add imploementation
            }
        }

    }
}
