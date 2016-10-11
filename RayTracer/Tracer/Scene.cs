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
        private Geometry[] geometries;
        private LinkedList<Transform> transforms = new LinkedList<Transform>();
        private Light[] lights;
        private List<Point3> vertices = new List<Point3>();
        private MyColor ambient;
        private Mat material;
        public Container Bvh;
        public MyColor defColor = new MyColor();

        public Scene()
        {
            Size = new Size();
            maxDepth = 5;
            transforms.AddFirst(new Scaling(new Point3(1, 1, 1)));
            material = new Mat();
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
            string command;
            while ((command = filereader.ReadLine()) != null)
                ExecuteCommand(command);

            ConvertToArray();


            filereader.Close();
        }

        private void ConvertToArray()
        {
            lights = tempLights.ToArray();
            geometries = tempGeos.ToArray();
            tempGeos.Clear();
            tempLights.Clear();
            transforms.Clear();
            vertices.Clear();
            transforms.Clear();

        }

        string CleanCommand(string command)
        {
            command = command.Trim();
            command = Regex.Replace(command, @"\s+", " ");
            return command;
        }

        List<Light> tempLights = new List<Light>();
        List<Geometry> tempGeos = new List<Geometry>();
        public void ExecuteCommand(string fullcommand)
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
            {
                param[i] = float.Parse(words[i + 1]);
            }
            switch (command)
            {

                case "defColor":
                    defColor = new MyColor(param[0],param[1],param[2]);
                    break;
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
                    Geometry tri = CreateShape(Geometry.TYPE.TRIANGLE, param);
                    tempGeos.Add(tri);
                    break;

                case "sphere":
                    Geometry sphere = CreateShape(Geometry.TYPE.SPHERE, param);
                    tempGeos.Add(sphere);
                    break;

                //transforms 
                case "pushTransform":
                    transforms.AddFirst(Utils.DeepClone(transforms.First()));
                    break;
                case "popTransform":
                    transforms.RemoveFirst();
                    break;
                case "translate":
                    transforms.First().Matrix = Matrix.Mul44x44(transforms.First().Matrix, (new Translation(param)).Matrix);
                    break;
                case "scale":
                    transforms.First().Matrix = Matrix.Mul44x44(transforms.First().Matrix, (new Scaling(param)).Matrix);
                    break;
                case "rotate":
                    transforms.First().Matrix = Matrix.Mul44x44(transforms.First().Matrix, (new Rotation(param)).Matrix);
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
                case "refIndex":
                    material.RefractIndex = param[0];
                    break;
                case "refValue":
                    material.RefractValue = param[0];
                    break;


                //light
                case "attenuation":
                    Attenuation = new Attenuation(param);
                    break;
                case "ambient":
                    ambient = new MyColor(param);
                    break;
                case "directional":
                    tempLights.Add(new DirectionalLight(param));
                    break;
                case "point":
                    tempLights.Add(new PointLight(param));
                    break;

                default:
                    break;
            }
        }


        Geometry CreateShape(Geometry.TYPE type, float[] param)
        {
            Geometry geo;
            if (type == Geometry.TYPE.SPHERE)
            {
                geo = CreateSphere(param);
            }
            else //if (type == Geometry.TYPE.TRIANGLE)
            {
                geo = CreateTriangle(param);
            }
            ApplyTransform(geo);
            ApplyMaterial(geo);
            ApplyAmbient(geo);

            return geo;
        }

        Sphere CreateSphere(float[] param)
        {
            Sphere sphere = new Sphere(param);

            return sphere;
        }

        Triangle CreateTriangle(float[] param)
        {
            Point3 a = vertices[(int)param[0]];
            Point3 b = vertices[(int)param[1]];
            Point3 c = vertices[(int)param[2]];

            Triangle tri = new Triangle(a, b, c);

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


        public Geometry[] Geometries
        {
            get { return geometries; }
            set { geometries = value; }
        }

        public Light[] Lights
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
            Console.WriteLine("Total Objects : " + Geometries.Length);
            Console.WriteLine("Total Lights  : " + Lights.Length);
            Console.WriteLine("Max Bounce    : " + MaxDepth);
            Console.WriteLine("Image Size    : " + Size.Width + " x " + Size.Height);
            Console.WriteLine("===================================================");
        }
    }
}
