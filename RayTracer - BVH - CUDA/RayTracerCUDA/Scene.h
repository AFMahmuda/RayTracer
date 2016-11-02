#pragma once
#include<vector>
#include<string>
#include<iostream>	
#include<fstream>	// file 

#include<regex>		//for cleaning and trimming string
#include<memory>

#include"Attenuation.h"

#include"Material.h"
#include"MyColor.h"

#include"Translation.h"
#include"Scaling.h"
#include"Rotation.h"

#include"Sphere.h"
#include"Triangle.h"

#include"PointLight.h"
#include"DirectionalLight.h"

#include"Camera.h"
#include"ViewPlane.h"
#include"Container.h"

//class Container;//forward declaration
class Scene
{
public:
	int maxDepth = 1;

	std::string outFileName = "default.bmp";
	std::shared_ptr<Container> bin;
	std::vector<std::shared_ptr<Geometry>> geometries;
	std::vector<std::shared_ptr<Light>> lights;

	Scene();
	Scene(std::string filename);

	Attenuation& getAtt() { return att; }
	~Scene();


private:

	int size[2];//pixel dimension
	std::vector<std::shared_ptr<Transform>> transforms;
	std::vector<std::shared_ptr<Point3>> vertices;
	MyColor ambient;
	Material material;
	Attenuation att;

	std::vector<std::string> splitString(std::string fullcommand, char delimiter);
	void parseCommand(std::string filename);
	std::string CleanCommand(std::string command);
	void executeCommand(std::string fullcommand);

	/*shape factory method*/
	std::shared_ptr<Geometry> createShape(Geometry::TYPE type, float* param);
	Sphere createSphere(float* param);
	Triangle createTriangle(float* param);
	void applyTransform(Geometry& shape);
	void applyMaterial(Geometry& shape);
	void applyAmbient(Geometry& shape);
	/*-----------------------*/
};
