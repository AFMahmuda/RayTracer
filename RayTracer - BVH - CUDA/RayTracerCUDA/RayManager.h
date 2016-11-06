#pragma once
#include<memory>
#include<vector>

#include"MyColor.h"
#include"Ray.h"
#include"Scene.h"


class RayManager
{
public:
	RayManager();
	static void traceRay(Ray& ray, std::shared_ptr<Container> bin);
	static MyColor& getColor(const Ray& ray, Scene& scene, int bounce);
	static MyColor getRefl(const Ray& ray, Scene& scene, int bounce);
	static	MyColor getRefr(const Ray& ray, Scene& scene, int bounce);
	static	MyColor calcColor(const Ray& ray, std::vector<std::shared_ptr< Light>> effectiveLights, Attenuation& attenuation);
	static	std::vector<std::shared_ptr< Light> >populateLights(const Ray& ray, std::vector < std::shared_ptr< Light>> allLights, std::shared_ptr<Container> bvh);

	~RayManager();
};

