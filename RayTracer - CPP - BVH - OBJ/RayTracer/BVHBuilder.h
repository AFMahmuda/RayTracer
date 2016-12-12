#pragma once
#include<vector>
#include<memory>

#include"BoxContainer.h"
#include"SphereContainer.h"
#include"ContainerFactory.h"

#include"Scene.h"
class BVHBuilder
{
private:
	static void buildTree(int id, std::vector<std::shared_ptr< Container>>& bins, std::vector<std::shared_ptr< Triangle>>& primitives, Container::TYPE const _type);
	/*cluster reduction function
	* n -> number of input clusters
	* return -> number of max output cluster*/
	//	static	int f(int n) { return (n <= 2) ? 1 : n / 2; }
	static	int f(int n) { return (n <= 2) ? 1 : powf(threshold, .5f) / 2.f * powf(n, .5f); }

	static	int getPivot(std::vector<std::shared_ptr< Triangle>>& geo);
	static void combineCluster(std::vector<std::shared_ptr< Container>>& bins, int limit);
	
public:
	bool isAAC;
	Container::TYPE type;
	static int threshold;
	BVHBuilder(Container::TYPE _type = Container::BOX, bool _isAAC = true, int _threshold = 20);
	std::shared_ptr<Container> buildBVH(std::vector<std::shared_ptr< Triangle>>& primitives);
	~BVHBuilder();
};

