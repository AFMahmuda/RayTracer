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
        private int maxDepth = 5;
        private Camera camera;
        private ViewPlane viewPlane;
        private LinkedList<Geometry> geometries = new LinkedList<Geometry>();
        private LinkedList<Transform> transforms = new LinkedList<Transform>();
        private LinkedList<Light> lights = new LinkedList<Light>();
        private List<Point3> vertices = new List<Point3>();
        private Attenuation attenuation;
        private MyColor ambient;
        private String outputFilename;


        public Scene()
        {
            Camera = new Camera();
            ViewPlane = new ViewPlane(1, 1, camera);
            Transforms.AddFirst(new Scaling(new Point3(1, 1, 1)));
            Material = new Material();
            Ambient = new MyColor(.2f, .2f, .2f);
            Attenuation = new Attenuation();

        }

        public Scene(String scenefile)
            : this()
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

                //geo
                case "sphere":
                    Sphere sphere = new Sphere(param);
                    ApplyTransform(sphere);
                    ApplyMaterial(sphere);
                    Geometries.AddLast(sphere);
                    break;
                case "tri":
                    Point3 a = vertices[(int)param[0]];
                    Point3 b = vertices[(int)param[1]];
                    Point3 c = vertices[(int)param[2]];

                    Triangle tri = new Triangle(a, b, c);
                    ApplyTransform(tri);
                    ApplyMaterial(tri);
                    Geometries.AddLast(tri);
                    break;
                case "maxverts":
                    vertices = new List<Point3>((int)param[0]);
                    break;
                case "vertex":
                    vertices.Add(new Point3(param));
                    break;

                //transforms
                case "pushTransform":
                    Transform transform = new Translation(Point3.ZERO);
                    transform.Matrix = Matrix.Mult44x44(transforms.First().Matrix, transform.Matrix);
                    transforms.AddFirst(transform);
                    break;
                case "popTransform":
                    transforms.RemoveFirst();
                    break;
                case "translate":
                    transforms.First().Matrix = Matrix.Mult44x44(transforms.First().Matrix, (new Translation(param)).Matrix);
                    break;
                case "scale":
                    transforms.First().Matrix = Matrix.Mult44x44(transforms.First().Matrix, (new Scaling(param)).Matrix);
                    break;
                case "rotate":
                    transforms.First().Matrix = Matrix.Mult44x44(transforms.First().Matrix, (new Rotation(param)).Matrix);
                    break;


                //material
                case "diffuse":
                    Material.Diffuse = Color.FromArgb((int)param[0] * 255, (int)param[1] * 255, (int)param[2] * 255);
                    break;
                case "specular":
                    Material.Specular = Color.FromArgb((int)param[0] * 255, (int)param[1] * 255, (int)param[2] * 255);
                    break;
                case "shininess":
                    Material.Shininess = param[0];
                    break;
                case "emission":
                    Material.Emission = Color.FromArgb((int)param[0] * 255, (int)param[1] * 255, (int)param[2] * 255);
                    break;


                //light
                case "attenuation":
                    Attenuation = new Attenuation(param);
                    break;
                case "ambient":
                    Ambient = new MyColor(param);
                    break;
                case "directional":
                    Lights.AddFirst(new DirectionalLight(param));
                    break;
                case "point":
                    Lights.AddFirst(new PointLight(param));
                    break;

                default:
                    break;
            }
        }


        private void ApplyTransform(Geometry shape)
        {
            shape.Transform = transforms.First();
        }


        private void ApplyMaterial(Geometry shape)
        {
            shape.Material = Material.Clone();
        }

        private void ApplyAmbient(Geometry shape)
        {
            shape.Ambient = Ambient;
        }


        public MyColor Ambient
        { get; set; }




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



        public Material Material
        {
            get;
            set;
        }

        internal Attenuation Attenuation
        {
            get { return attenuation; }
            set { attenuation = value; }
        }

        public Size Size
        {
            get { return size; }
            set { size = value; }
        }
    }
}
