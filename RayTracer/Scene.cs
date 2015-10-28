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


        private LinkedList<Geometry> geometries = new LinkedList<Geometry>();
        private LinkedList<Transform> transforms = new LinkedList<Transform>();
        private LinkedList<Light> lights = new LinkedList<Light>();
        private List<Point3> vertices = new List<Point3>();



        public Scene()
        {
            Size = new Size();
            MaxDepth = 5;
            Camera = new Camera();
            ViewPlane = new ViewPlane(1, 1, Camera);
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
                    ApplyAmbient(sphere);
                    Geometries.AddLast(sphere);
                    break;
                case "tri":
                    Point3 a = vertices[(int)param[0]];
                    Point3 b = vertices[(int)param[1]];
                    Point3 c = vertices[(int)param[2]];

                    Triangle tri = new Triangle(a, b, c);
                    ApplyTransform(tri);
                    ApplyMaterial(tri);
                    ApplyAmbient(tri);
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
                    Material.Diffuse = new MyColor(param[0], param[1], param[2]);
                    break;
                case "specular":
                    Material.Specular = new MyColor(param[0], param[1], param[2]);
                    break;
                case "emission":
                    Material.Emission = new MyColor(param[0], param[1], param[2]);
                    break;
                case "shininess":
                    Material.Shininess = param[0];
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
            shape.Transform = transforms.First().Clone();
        }

        private void ApplyMaterial(Geometry shape)
        {
            shape.Material = Material.Clone();
        }

        private void ApplyAmbient(Geometry shape)
        {
            shape.Ambient = Ambient.Clone();
        }

        public MyColor Ambient
        {
            get;
            set;
        }

        public Camera Camera
        {
            get;
            set;
        }

        public ViewPlane ViewPlane
        {
            get;
            set;
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
            get;
            set;
        }

        public String OutputFilename
        {
            get;
            set;
        }

        public Material Material
        {
            get;
            set;
        }

        internal Attenuation Attenuation
        {
            get;
            set;
        }

        public Size Size
        {
            get;
            set;
        }
    }
}
