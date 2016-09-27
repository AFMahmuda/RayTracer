using RayTracer.Common;
using RayTracer.Shape;
using RayTracer.Lighting;
using RayTracer.Material;
using RayTracer.BVH;
using RayTracer.Transformation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace RayTracer.Tracer
{
    [Serializable]
    public class Scene
    {


        private List<Geometry> geometries = new List<Geometry>();
        private LinkedList<Transform> transforms = new LinkedList<Transform>();
        private List<Light> lights = new List<Light>();
        private List<Point3> vertices = new List<Point3>();
        private MyColor ambient;
        private Mat material;
        public Container Bvh;

        public Scene()
        {
            Size = new Size();
            maxDepth = 5;
            transforms.AddFirst(new Scaling(new Point3(1, 1, 1)));
            material = new Mat();
            ambient = new MyColor(.2, .2, .2);
            Attenuation = new Attenuation();
            OutputFilename = "default.bmp";
        }

        public String SceneFile { get; set; }
        public Scene(String scenefile)
            : this()
        {
            ParseCommand(scenefile);
        }

        public void ParseCommand(String scenefile)
        {
            SceneFile = scenefile;
            StreamReader filereader = new StreamReader(scenefile);
            string command;
            while ((command = filereader.ReadLine()) != null)
                ExecuteCommand(command);
            filereader.Close();
        }


        string CleanCommand(string command)
        {
            command = command.Trim();
            command = Regex.Replace(command, @"\s+", " ");
            return command;
        }

        public void ExecuteCommand(String fullcommand)
        {

            if (fullcommand.Contains('#'))
                return;

            fullcommand = CleanCommand(fullcommand);
            String[] words = fullcommand.Split(' ');
            String command = words[0];

            if (command.Equals("output"))
            {
                OutputFilename = words[1] + ".bmp";
                return;
            }

            double[] param = new double[words.Length - 1];
            for (int i = 0; i < param.Length; i++)
            {
                param[i] = double.Parse(words[i + 1]);
            }
            switch (command)
            {
                case "size":
                    Size = new Size((int)param[0], (int)param[1]);
                    break;
                case "camera":
                    if (Camera.Instance != null) { break; }
                    Camera.Instance = new Camera(param);
                    ViewPlane.Instance = new ViewPlane(Size.Width, Size.Height);
                    break;
                case "maxdepth":
                    maxDepth = (int)param[0];
                    break;


                case "maxverts":
                    vertices = new List<Point3>((int)param[0]);
                    break;
                case "vertex":
                    vertices.Add(new Point3(param));
                    break;

                //geometry
                case "tri":
                    Triangle tri = CrerateTriangle(param);
                    Geometries.Add(tri);
                    break;

                case "sphere":
                    Sphere sphere = CreateSphere(param);
                    Geometries.Add(sphere);
                    break;


                //transforms 
                case "pushTransform":
                    transforms.AddFirst(Utils.DeepClone(transforms.First()));
                    break;
                case "popTransform":
                    transforms.RemoveFirst();
                    break;
                case "translate":
                    transforms.First().Matrix = MyMatrix.Mult44x44(transforms.First().Matrix, (new Translation(param)).Matrix);
                    break;
                case "scale":
                    transforms.First().Matrix = MyMatrix.Mult44x44(transforms.First().Matrix, (new Scaling(param)).Matrix);
                    break;
                case "rotate":
                    transforms.First().Matrix = MyMatrix.Mult44x44(transforms.First().Matrix, (new Rotation(param)).Matrix);
                    break;

                //material
                case "diffuse":
                    material.Diffuse = new MyColor(param[0], param[1], param[2]);
                    break;
                case "specular":
                    material.Specular = new MyColor(param[0], param[1], param[2]);
                    break;
                case "emission":
                    material.Emission = new MyColor(param[0], param[1], param[2]);
                    break;
                case "shininess":
                    material.Shininess = param[0];
                    break;


                //light
                case "attenuation":
                    Attenuation = new Attenuation(param);
                    break;
                case "ambient":
                    ambient = new MyColor(param);
                    break;
                case "directional":
                    Lights.Add(new DirectionalLight(param));
                    break;
                case "point":
                    Lights.Add(new PointLight(param));
                    break;

                default:
                    break;
            }
        }


        Sphere CreateSphere(double[] param)
        {
            Sphere sphere = new Sphere(param);

            ApplyTransform(sphere);
            ApplyMaterial(sphere);
            ApplyAmbient(sphere);

            return sphere;
        }

        Triangle CrerateTriangle(double[] param)
        {
            Point3 a = vertices[(int)param[0]];
            Point3 b = vertices[(int)param[1]];
            Point3 c = vertices[(int)param[2]];

            Triangle tri = new Triangle(a, b, c);

            ApplyTransform(tri);
            ApplyMaterial(tri);
            ApplyAmbient(tri);
            
            return tri;
        }

        private void ApplyTransform(Geometry shape)
        {
            shape.Trans = Utils.DeepClone(transforms.First());
        }

        private void ApplyMaterial(Geometry shape)
        {
            shape.Material = Utils.DeepClone(material);
        }

        private void ApplyAmbient(Geometry shape)
        {
            shape.Ambient = Utils.DeepClone(ambient);
        }


        public List<Geometry> Geometries
        {
            get { return geometries; }
            set { geometries = value; }
        }

        public List<Light> Lights
        {
            get { return lights; }
        }



        int maxDepth;
        public int MaxDepth
        { get { return maxDepth; } }

        public string OutputFilename
        { get; set; }


        public Attenuation Attenuation
        { get; set; }

        public Size Size
        { get; set; }

        public void ShowInformation()
        {
            Console.WriteLine("=======Scene Information===========================");
            //Camera.ShowInformation();
            Console.WriteLine("Total Objects : " + Geometries.Count);
            Console.WriteLine("Total Lights  : " + Lights.Count);
            Console.WriteLine("Max Bounce    : " + MaxDepth);
            Console.WriteLine("Image Size    : " + Size.Width + " x " + Size.Height);
            Console.WriteLine("===================================================");
        }
    }
}
