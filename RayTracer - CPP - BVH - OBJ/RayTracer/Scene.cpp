#include<sstream>

#include "Scene.h"

#include"Camera.h"
#include"ViewPlane.h"
Scene::Scene()
{
	att = Attenuation(new float[3]{ 1,0,0 });
}

Scene::Scene(std::string filename) :Scene()
{
	parseFile(filename);
	vertices.clear();
}

void Scene::parseFile(std::string filename)
{
	std::string line;
	std::ifstream myfile(filename);
	if (myfile.is_open())
	{
		while (getline(myfile, line))
		{
			executeCommand(line);
		}
		myfile.close();
	}
	else std::cout << "Unable to open file";
}

std::string Scene::CleanCommand(std::string command)
{
	command = std::regex_replace(command, std::regex("\t| + "), " "); //replace tab and multiple spaces w/ space
	command = std::regex_replace(command, std::regex("^ +| +$"), ""); //replce leading and trailing space with nothing
	return command;
}

std::vector<std::string> Scene::splitString(std::string fullcommand, char delimiter) {
	std::vector<std::string> elems;
	std::stringstream ss(fullcommand);
	std::string item;
	while (std::getline(ss, item, delimiter)) {
		if (item.length() > 0) {
			elems.push_back(item);
		}
	}
	return elems;
}

void Scene::executeCommand(std::string fullcommand)
{

	if (fullcommand.compare("") == 0)
		return;
	if (fullcommand.find('#') != std::string::npos)
		return;

	fullcommand = CleanCommand(fullcommand);

	std::vector<std::string> words = splitString(fullcommand, ' ');
	std::string command = words[0];
	std::vector<float> param;

	//	std::transform(command.begin(), command.end(), command.begin(), ::tolower);

	//geometry
	if (command.compare("v") == 0)
	{
		for (int i = 0; i < words.size() - 1; i++)
		{
			param.push_back(stof(words[i + 1]));
		}
		vertices.push_back(std::make_shared<Vec3>(&param[0], 1));
	}

	else if (command.compare("f") == 0)
	{
		for (int i = 0; i < words.size() - 1; i++)
		{
			std::string temp = splitString(words[i + 1], '/')[0];
			param.push_back(stof(temp));
		}
		auto geo = createShape(&param[0]);
		geometries.push_back(geo);
	}
	else if (command.compare("usemtl") == 0)
	{
		for each (auto var in material)
		{
			if (var->name == words[1])
				currMat = std::find(material.begin(), material.end(), var);
		}
	}

	//material
	else if (command.compare("newmtl") == 0)
	{
		material.push_back(std::make_shared<Material>());
		material.back()->name = words[1];
	}
	else if (command.compare("Ka") == 0)
	{
		for (int i = 0; i < 3; i++) { param.push_back(stof(words[i + 1])); }
		material.back()->reflVal = MyColor(param[0], param[1], param[2]);
	}

	else if (command.compare("Kd") == 0)
	{
		for (int i = 0; i < 3; i++) { param.push_back(stof(words[i + 1])); }
		material.back()->diffuse = (MyColor(param[0], param[1], param[2]));
	}

	else if (command.compare("Ks") == 0)
	{
		for (int i = 0; i < 3; i++) { param.push_back(stof(words[i + 1])); }
		material.back()->specular = (MyColor(param[0], param[1], param[2]));
	}

	else if (command.compare("Ke") == 0)
	{
		for (int i = 0; i < 3; i++) { param.push_back(stof(words[i + 1])); }
		material.back()->emmission = (MyColor(param[0], param[1], param[2]));
	}

	else if (command.compare("Ns") == 0)
	{
		material.back()->setShininess(stof(words[1]));
	}

	else if (command.compare("Ni") == 0)
	{
		material.back()->setrefIndex(stof(words[1]));

	}

	else if (command.compare("d") == 0)
	{
		material.back()->setRefValue(stof(words[1]));

	}

	//else if (command.compare("vn") == 0)
	//{
	//	//ignore vector normal 
	//}

	else if (command.compare("output") == 0)
	{
		outFileName = words[1] + ".bmp";

	}

	else if (command.compare("geo") == 0)
	{
		parseFile(words[1]);
	}

	else if (command.compare("mtllib") == 0)
	{
		parseFile(words[1]);
	}

	else if (command.compare("size") == 0) {
		for (int i = 0; i < words.size() - 1; i++)
		{
			param.push_back(stof(words[i + 1]));
		}
		size[0] = (int)param[0];
		size[1] = (int)param[1];

	}

	else if (command.compare("camera") == 0) {
		for (int i = 0; i < words.size() - 1; i++)
		{
			param.push_back(stof(words[i + 1]));
		}
		Camera::Instance()->Init(&param[0]);
		ViewPlane::Instance()->Init(size[0], size[1]);

	}
	else if (command.compare("maxdepth") == 0)
	{
		for (int i = 0; i < words.size() - 1; i++)
		{
			param.push_back(stof(words[i + 1]));
		}
		maxDepth = (int)param[0];
	}

	else if (command.compare("point") == 0)
	{
		std::vector<float> param;
		for (int i = 0; i < words.size() - 1; i++)
		{
			param.push_back(stof(words[i + 1]));
		}
		lights.push_back(std::make_shared<PointLight>(new Vec3(param[0], param[1], param[2], 1), new MyColor(param[3], param[4], param[5])));

	}

	else if (command.compare("directional") == 0)
	{
		std::vector<float> param;
		for (int i = 0; i < words.size() - 1; i++)
		{
			param.push_back(stof(words[i + 1]));
		}
		lights.push_back(std::make_shared<DirectionalLight>(new Vec3(param[0], param[1], param[2], 0), new MyColor(param[3], param[4], param[5])));

	}

}

std::shared_ptr<Triangle> Scene::createShape(float * param)
{
	return std::make_shared<Triangle>(createTriangle(param));
}

Triangle Scene::createTriangle(float * param)
{
	Vec3 p[3];
	for (size_t i = 0; i < 3; i++)
	{
		if ((int)param[i] < 0)
			param[i] += vertices.size() + 1;
		p[i] = *vertices[(int)param[i] - 1];
	}
	Triangle tri = Triangle(p[0], p[1], p[2]);
	tri.mat = **currMat;

	return tri;
}


Scene::~Scene()
{
}
