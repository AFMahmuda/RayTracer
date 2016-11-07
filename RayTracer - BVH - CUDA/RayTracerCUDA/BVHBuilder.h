#pragma once
#include"BoxContainer.h"
#include"SphereContainer.h"
#include"ContainerFactory.h"
#include"RadixSort.h"
#include"Scene.h"
#include<vector>

#include<memory>
class BVHBuilder
{
public:
	bool isAAC;
	Container::TYPE type;
	int threshold = 4; //4 or 20, smaller = higher Q. 
	BVHBuilder(Container::TYPE _type = Container::BOX, bool _isAAC = true);
	void BuildBVH(Scene& scene);
	std::vector<std::shared_ptr< Container>> BuildTree(std::vector<	std::shared_ptr< Triangle>> primitives);
	/*cluster reduction function
	* n -> number of input clusters
	* return -> number of max output cluster
	*/
	int f(int n) { return (n <= 2) ? 1 : n / 2; }
	/*list partition function,
	* pivot is the frst bit 'flip'
	* ex : [0] 00000111
	*      [1] 00001000
	*      [2] 00001000
	*      [3] 00100000
	*      pivot -> 3 (flipped on third element 000xxxxx to 001xxxxx)
	*/
	int getPivot(std::vector<std::shared_ptr< Triangle>> geo);

	//combine [bins] cluster to [limit] cluster
	std::vector<std::shared_ptr< Container>> CombineCluster(std::vector<std::shared_ptr< Container>> bins, int limit);

	~BVHBuilder();
};

