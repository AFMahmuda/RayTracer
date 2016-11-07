#include "Scene.h"

Scene::Scene()
{
	att = Attenuation(new float[3]{ 1,0,0 });
}

Scene::Scene(std::string filename) :Scene()
{
	parseFile(filename);
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
		vertices.clear();
	}
	else std::cout << "Unable to open file";
}

std::string Scene::CleanCommand(std::string command)
{
	command = std::regex_replace(command, std::regex("\t"), " "); //replace tab w/ space
	command = std::regex_replace(command, std::regex(" + "), " "); //replace multiple spaces w/ sigle space
	command = std::regex_replace(command, std::regex("^ +| +$"), ""); //replce leading and trailing space with nothing
	return command;
}

std::vector<std::string> Scene::splitString(std::string fullcommand, char delimiter) {
	std::vector<std::string> results;
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

void Scene::executeCommand(std::string fullcommand)
{
	if (fullcommand.compare("") == 0)
		return;
	if (fullcommand.find('#') != std::string::npos)
		return;

	fullcommand = CleanCommand(fullcommand);

	std::vector<std::string> words = splitString(fullcommand, ' ');
	std::string command = words[0];
	std::transform(command.begin(), command.end(), command.begin(), ::tolower);

	if (command.compare("output") == 0)
	{
		outFileName = words[1] + ".bmp";
		return;
	}

	std::vector<float> param;
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
		vertices.push_back(std::make_shared<Vec3>(&param[0], 1));
		return;
	}

	//geometry
	if (command.compare("tri") == 0)
	{
		auto geo = createShape(&param[0]);
		geometries.push_back(geo);
		return;

	}


	//material

	if (command.compare("diffuse") == 0)
	{
		material.back()->diffuse = (MyColor(param[0], param[1], param[2]));
		return;
	}

	if (command.compare("specular") == 0)
	{
		material.back()->specular = (MyColor(param[0], param[1], param[2]));
		return;
	}
	if (command.compare("emission") == 0)
	{
		material.back()->emmission = (MyColor(param[0], param[1], param[2]));
		return;
	}
	if (command.compare("shininess") == 0)
	{
		material.back()->setShininess(param[0]);
		return;
	}

	if (command.compare("refindex") == 0)
	{
		material.back()->setrefIndex(param[0]);
		return;
	}
	if (command.compare("refvalue") == 0)
	{
		material.back()->setRefValue(param[0]);
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
		material.back()->ambient = MyColor(param[0], param[1], param[2]);
		return;
	}
	if (command.compare("directional") == 0)
	{
		lights.push_back(std::make_shared<DirectionalLight>(new Vec3(param[0], param[1], param[2], 0), new MyColor(param[3], param[4], param[5])));
		return;
	}
	if (command.compare("point") == 0)
	{
		lights.push_back(std::make_shared<PointLight>(new Vec3(param[0], param[1], param[2], 1), new MyColor(param[3], param[4], param[5])));
		return;
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
		p[i] = *vertices[(int)param[i]];
	}
	Triangle tri = Triangle(p[0], p[1], p[2]);
	applyMaterial(tri);
	return tri;
}


void Scene::applyMaterial(Triangle & shape)
{
	shape.mat = *material.back();
}

Scene::~Scene()
{
}
