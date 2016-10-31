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

#include"Camera.h"
#include"ViewPlane.h"

#include<vector>
#include<string>
#include<iostream>
#include<fstream>

#include<regex>
#include<memory>

using namespace std;
class Scene
{
public:
	int maxDepth = 5;
	int size[2];
	shared_ptr<Container > container;

	vector<	shared_ptr< Geometry>> geometries;
	vector< shared_ptr<Light>> light;
	vector< shared_ptr<Transform>> transforms;
	vector< shared_ptr<Point3>> vertices;

	MyColor ambient = MyColor();
	Material material = Material();
	Attenuation att = Attenuation();
	std::string outFileName = "default.bmp";
	Scene()
	{

		auto trans = make_shared<Transform>(Translation(0,0,0));
		transforms.push_back(trans);		
		ambient = MyColor();
		att = Attenuation();
		material = Material();
	}

	Scene(string filename) :Scene()
	{
		parseCommand(filename);
	}

	void parseCommand(string filename)
	{

		string line;
		ifstream myfile(filename);
		if (myfile.is_open())
		{
			while (getline(myfile, line))
			{
				executeCommand(line);
			}
			myfile.close();
			shared_ptr<Triangle> tri = static_pointer_cast<Triangle>(geometries[0]);
			shared_ptr<Triangle> tri2 = static_pointer_cast<Triangle>(geometries[1]);
			shared_ptr<Sphere> sph = static_pointer_cast<Sphere>(geometries[2]);

		}
		else cout << "Unable to open file";
	}

	string CleanCommand(string command)
	{

		command = regex_replace(command, regex("\t"), " "); //replace tab w/ space
		command = regex_replace(command, regex("^ +| +$"), ""); //replce leading and trailing space with nothing
		return command;
	}

	vector<string> splitString(string fullcommand, char delimiter) {
		vector<string> results;
		size_t pos = 0;
		std::string token;
		while ((pos = fullcommand.find(delimiter)) != std::string::npos) {
			token = fullcommand.substr(0, pos);
			results.push_back(token);
			fullcommand.erase(0, pos + 1);
		}
		results.push_back(fullcommand);
		fullcommand.erase(0, pos);
		return results;
	}

	void executeCommand(string fullcommand)
	{
		if (fullcommand.compare("") == 0)
			return;
		if (fullcommand.find('#') != string::npos)
			return;

		fullcommand = CleanCommand(fullcommand);

		vector<string> words = splitString(fullcommand, ' ');
		string command = words[0];
		std::transform(command.begin(), command.end(), command.begin(), ::tolower);

		if (command.compare("output") == 0)
		{
			outFileName = words[1] + ".bmp";
			return;
		}

		vector<float> param;
		for (int i = 0; i < words.size() - 1; i++)
		{
			param.push_back(stof(words[i + 1]));
		}


		//	case "defColor":
		//		defColor = new MyColor(param[0], param[1], param[2]);
		//		break;
		if (command.compare("size") == 0) {
			size[0] = (int)param[0];
			size[1] = (int)param[1];
			return;
		}

		if (command.compare("camera") == 0) {

			Camera::Instance()->Init(&param[0]);
			ViewPlane::Instance()->Init(size[0], size[1]);

			return;
		}
		if (command.compare("maxdepth") == 0)
		{
			maxDepth = (int)param[0];
			return;
		}

		if (command.compare("maxvertex") == 0)
		{
			//no need		
			//vertices.reserve((int)param[0]);
			return;
		}
		if (command.compare("vertex") == 0)
		{
			vertices.push_back(make_shared<Point3>(Point3(&param[0])));
			return;
		}

		//geometry
		if (command.compare("tri") == 0)
		{
			auto geo = make_shared<Triangle>(createTriangle(&param[0]));
			geometries.push_back(geo);
			return;

		}
		if (command.compare("sphere") == 0)
		{
			auto geo = make_shared<Sphere>(createSphere(&param[0]));
			geometries.push_back(geo);
			return;
		}


		//transforms 
		if (command.compare("pushtransform") == 0)
		{
			auto trans = make_shared<Transform>(transforms.back());
			transforms.push_back(trans);
			return;
		}
		if (command.compare("poptransform") == 0)
		{
			transforms.pop_back();
			return;
		}

		if (command.compare("translate") == 0)
		{
			/*Transform trans = Translation(&param[0]);
			Transform last = *transforms.back();
			last.matrix = Matrix::Mul44x44(last.matrix, (trans.matrix));*/
			return;
		}
		if (command.compare("scale") == 0)
		{
			/*Transform trans = Scaling(&param[0]);
			Transform last = *transforms.back();
			last.matrix = Matrix::Mul44x44(last.matrix, (trans.matrix));*/
			return;
		}
		if (command.compare("rotate") == 0)
		{
			/*Transform trans = Rotation(&param[0]);
			Transform last = *transforms.back();
			last.matrix = Matrix::Mul44x44(last.matrix, trans.matrix);*/
			return;
		}
		//material

		if (command.compare("diffuse") == 0)
		{
			material.Diffuse = make_shared<MyColor>(MyColor(param[0], param[1], param[2]));
			return;
		}

		if (command.compare("specular") == 0)
		{
			material.Specular = make_shared<MyColor>(MyColor(param[0], param[1], param[2]));
			return;
		}
		if (command.compare("emission") == 0)
		{

			material.Emmission = make_shared<MyColor>(MyColor(param[0], param[1], param[2]));
			return;
		}
		if (command.compare("shininess") == 0)
		{
			material.setShininess(param[0]);
			return;
		}

		if (command.compare("refindex") == 0)
		{
			material.setrefIndex(param[0]);
			return;
		}
		if (command.compare("refvalue") == 0)
		{
			material.setRefValue(param[0]);
			return;
		}

		//		//light
		if (command.compare("attenuation") == 0)
		{
			att = Attenuation(&param[0]);
			return;
		}
		if (command.compare("ambient") == 0)
		{
			ambient = MyColor(param[0], param[1], param[2]);
			return;
		}
		if (command.compare("directional") == 0)
		{
			light.push_back(make_shared<DirectionalLight>(DirectionalLight(new Vec3(&param[0]), new MyColor(param[3], param[4], param[5]))));
			return;
		}
		if (command.compare("point") == 0)
		{
			light.push_back(make_shared<PointLight>(PointLight(new Point3(&param[0]), new MyColor(param[3], param[4], param[5]))));
			return;
		}
	}

	Sphere createSphere(float* param)
	{
		Sphere sphere = Sphere(param);
		applyTransform(&sphere);

		applyMaterial(sphere);
		applyAmbient(&sphere);
		return sphere;
	}

	Triangle createTriangle(float* param)
	{
		Point3 p[3];
		for (size_t i = 0; i < 3; i++)
		{
			p[i] = Point3(*vertices[(int)param[i]]);
			p[i] = Matrix::Mul44x41(transforms.back()->matrix, p[i]);
			/*p[i].h = 1;*/
		}

		Triangle tri = Triangle(p[0], p[1], p[2]);
		applyMaterial(tri);
		applyAmbient(&tri);
		return tri;
	}

	void applyTransform(Geometry* shape)
	{
		auto currTrans = make_shared<Transform>(transforms.back());
		shape->setTrans(*currTrans);
	}

	void applyMaterial(Geometry& shape)
	{
		shape.mat = *make_shared<Material>(material);
	}

	void applyAmbient(Geometry* shape)
	{
		shape->ambient = *make_shared<MyColor>(ambient);
	}



	~Scene();
};

