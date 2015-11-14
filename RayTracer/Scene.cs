using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;

namespace RayTracer
{
    [Serializable]
    public class Scene
    {


        private List<Geometry> geometries = new List<Geometry>();
        private LinkedList<Transform> transforms = new LinkedList<Transform>();
        private List<Light> lights = new List<Light>();
        private List<Point3> vertices = new List<Point3>();
        private MyColor ambient;
        private Material material;



        public Scene()
        {
            Size = new Size();
            MaxDepth = 5;
            Camera = new Camera();
            transforms.AddFirst(new Scaling(new Point3(1, 1, 1)));
            material = new Material();
            ambient = new MyColor(.2f, .2f, .2f);
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
            String command;
            while ((command = filereader.ReadLine()) != null)
                ExecuteCommand(command);
            filereader.Close();        
        }


        String CleanCommand(String command)
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
                    Geometries.Add(sphere);
                    break;
                case "tri":
                    Point3 a = vertices[(int)param[0]];
                    Point3 b = vertices[(int)param[1]];
                    Point3 c = vertices[(int)param[2]];

                    Triangle tri = new Triangle(a, b, c);
                    ApplyTransform(tri);
                    ApplyMaterial(tri);
                    ApplyAmbient(tri);
                    Geometries.Add(tri);
                    break;

                case "maxverts":
                    vertices = new List<Point3>((int)param[0]);
                    break;
                case "vertex":
                    vertices.Add(new Point3(param));
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

        private void ApplyTransform(Geometry shape)
        {
            shape.Transform = Utils.DeepClone(transforms.First());
            MyMatrix foo = shape.Transform.Matrix.Inverse;
        }

        private void ApplyMaterial(Geometry shape)
        {
            shape.Material = Utils.DeepClone(material);
        }

        private void ApplyAmbient(Geometry shape)
        {
            shape.Ambient = Utils.DeepClone(ambient);
        }

        public Camera Camera
        { get; set; }


        public List<Geometry> Geometries
        {
            get { return geometries; }
        }

        public List<Light> Lights
        {
            get { return lights; }
        }

        public int MaxDepth
        { get; set; }

        public String OutputFilename
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
            Console.WriteLine("Image Size    : " + Size.Width +" x " + Size.Height);
            Console.WriteLine("===================================================");
        }
    }
}
