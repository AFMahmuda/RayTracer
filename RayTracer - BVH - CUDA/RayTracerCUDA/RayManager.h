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
	void traceRay(Ray& ray, Container& bin);
	MyColor& getColor(const Ray& ray, Scene& scene, int bounce);
	MyColor getRefl(const Ray& ray, Scene& scene, int bounce);
	MyColor getRefr(const Ray& ray, Scene& scene, int bounce);
	MyColor calcColor(const Ray& ray, std::vector<std::shared_ptr< Light>> effectiveLights, Attenuation& attenuation);
	std::vector<std::shared_ptr< Light> >populateLights(const Ray& ray, std::vector < std::shared_ptr< Light>> allLights, Container& bvh);

	~RayManager();
};

