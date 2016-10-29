#pragma once
#include"Attenuation.h"

#include"Vec3.h"
#include"Point3.h"

#include"Material.h"
#include"MyColor.h"

#include"Translation.h"
#include"Scaling.h"
#include"Rotation.h"

#include"Sphere.h"
#include"Triangle.h"
#include"Container.h"

#include"PointLight.h"
#include"DirectionalLight.h"

#include<vector>
#include<string>

class Scene
{




public:
	int maxDepth = 5;
	int size[2];
	Container * container;

	std::vector< Geometry*> geometries;
	std::vector< Light*> light;
	std::vector< Transform*> transforms;
	std::vector< Point3*> vertices;

	MyColor ambient = MyColor();
	Material material = Material();
	Attenuation att = Attenuation();
	std::string outFileName = "default.bmp";
	Scene()
	{
		transforms.push_back(new Translation());
	}

	Scene(std::string filename) :Scene()
	{
		parseCommand(filename);
	}

	void parseCommand(std::string filename
		)
	{
		//StreamReader filereader = new StreamReader(scenefile);
		//string command;
		//while ((command = filereader.ReadLine()) != null)
		//	ExecuteCommand(command);

		//ConvertToArray();


		//filereader.Close();


	}

	std::string CleanCommand(std::string command)
	{
		//command = command.Trim();
		//command = Regex.Replace(command, @"\s + ", " ");
		return command;
	}

	void executeCommand(std::string command)
	{
		//	if (fullcommand.Contains('#'))
		//		return;

		//	fullcommand = CleanCommand(fullcommand);
		//	String[] words = fullcommand.Split(' ');
		//	String command = words[0];

		//	if (command.Equals("output"))
		//	{
		//		OutputFilename = words[1] + ".bmp";
		//		return;
		//	}

		//	float[] param = new float[words.Length - 1];
		//	for (int i = 0; i < param.Length; i++)
		//	{
		//		param[i] = float.Parse(words[i + 1]);
		//	}
		//	switch (command)
		//	{

		//	case "defColor":
		//		defColor = new MyColor(param[0], param[1], param[2]);
		//		break;
		//	case "size":
		//		Size = new Size((int)param[0], (int)param[1]);
		//		break;
		//	case "camera":
		//		if (Camera.Instance != null) { break; }
		//		Camera.Instance = new Camera(param);
		//		ViewPlane.Instance = new ViewPlane(Size.Width, Size.Height);
		//		break;
		//	case "maxdepth":
		//		maxDepth = (int)param[0];
		//		break;


		//	case "maxverts":
		//		vertices = new List<Point3>((int)param[0]);
		//		break;
		//	case "vertex":
		//		vertices.Add(new Point3(param));
		//		break;

		//		//geometry
		//	case "tri":
		//		Geometry tri = CreateShape(Geometry.TYPE.TRIANGLE, param);
		//		tempGeos.Add(tri);
		//		break;

		//	case "sphere":
		//		Geometry sphere = CreateShape(Geometry.TYPE.SPHERE, param);
		//		tempGeos.Add(sphere);
		//		break;

		//		//transforms 
		//	case "pushTransform":
		//		transforms.AddFirst(Utils.DeepClone(transforms.First()));
		//		break;
		//	case "popTransform":
		//		transforms.RemoveFirst();
		//		break;
		//	case "translate":
		//		transforms.First().Matrix = Matrix.Mul44x44(transforms.First().Matrix, (new Translation(param)).Matrix);
		//		break;
		//	case "scale":
		//		transforms.First().Matrix = Matrix.Mul44x44(transforms.First().Matrix, (new Scaling(param)).Matrix);
		//		break;
		//	case "rotate":
		//		transforms.First().Matrix = Matrix.Mul44x44(transforms.First().Matrix, (new Rotation(param)).Matrix);
		//		break;

		//		//material
		//	case "diffuse":
		//		material.Diffuse = new MyColor(param[0], param[1], param[2]);
		//		break;
		//	case "specular":
		//		material.Specular = new MyColor(param[0], param[1], param[2]);
		//		break;
		//	case "emission":
		//		material.Emission = new MyColor(param[0], param[1], param[2]);
		//		break;
		//	case "shininess":
		//		material.Shininess = param[0];
		//		break;
		//	case "refIndex":
		//		material.RefractIndex = param[0];
		//		break;
		//	case "refValue":
		//		material.RefractValue = param[0];
		//		break;


		//		//light
		//	case "attenuation":
		//		Attenuation = new Attenuation(param);
		//		break;
		//	case "ambient":
		//		ambient = new MyColor(param);
		//		break;
		//	case "directional":
		//		tempLights.Add(new DirectionalLight(param));
		//		break;
		//	case "point":
		//		tempLights.Add(new PointLight(param));
		//		break;

		//	default:
		//		break;
		//	}
	}

	Geometry* createShape(Geometry::TYPE type, float* params)
	{
		Geometry* geo;
		if (type == Geometry::SPHERE)
		{
			geo = createSphere(params);
			applyTransform(geo);

		}
		else //if (type == Geometry.TYPE.TRIANGLE)
		{
			geo = createTriangle(params);
			//transform already aplied in each vertices
		}
		applyMaterial(geo);
		applyAmbient(geo);

		return geo;
	}

	Sphere* createSphere(float* param)
	{
		Sphere sphere = Sphere(param);

		return &sphere;
	}

	Triangle* createTriangle(float* param)
	{
		Point3* a = vertices[(int)param[0]];
		Point3* b = vertices[(int)param[1]];
		Point3* c = vertices[(int)param[2]];


		//a = Matrix.Mul44x41(transforms.First().Matrix, new Vec3(a), 1);
		//b = Matrix.Mul44x41(transforms.First().Matrix, new Vec3(b), 1);
		//c = Matrix.Mul44x41(transforms.First().Matrix, new Vec3(c), 1);

		//Triangle tri = Triangle(a, b, c);

		Triangle tri = Triangle(0, 0, 0);

		return &tri;
	}

	void applyTransform(Geometry* shape)
	{
		//		shape.Trans = Utils.DeepClone(transforms.First());
	}

	void applyMaterial(Geometry* shape)
	{
		//		shape.Material = Utils.DeepClone(material);
	}

	void applyAmbient(Geometry* shape)
	{
		//		shape.Ambient = Utils.DeepClone(ambient);
	}



	~Scene();
};

