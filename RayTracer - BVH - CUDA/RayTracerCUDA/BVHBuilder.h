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
	static const int threshold = 20;
	BVHBuilder(Container::TYPE _type = Container::BOX, bool _isAAC = true);
	void BuildBVH(Scene& scene);
	static void buildTree(std::vector<std::shared_ptr< Container>>& bins, std::vector<	std::shared_ptr< Triangle>>& primitives, Container::TYPE _type);
	/*cluster reduction function
	* n -> number of input clusters
	* return -> number of max output cluster
	*/
	static	int f(int n) { return (n <= 2) ? 1 : n / 2; }
	/*list partition function,
	* pivot is the frst bit 'flip'
	* ex : [0] 00000111
	*      [1] 00001000
	*      [2] 00001000
	*      [3] 00100000
	*      pivot -> 3 (flipped on third element 000xxxxx to 001xxxxx)
	*/
	static	int getPivot(std::vector<std::shared_ptr< Triangle>>& geo);

	//combine [bins] cluster to [limit] cluster
	static void CombineCluster(std::vector<std::shared_ptr< Container>>& bins, int limit);
	~BVHBuilder();
};

