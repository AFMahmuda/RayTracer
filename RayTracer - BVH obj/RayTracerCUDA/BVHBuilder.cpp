#include "BVHBuilder.h"

#include <thread>

#include"RadixSort.h"

int BVHBuilder::threshold = 20;

BVHBuilder::BVHBuilder(Container::TYPE _type, bool _isAAC, int _threshold) {
	type = _type;
	isAAC = _isAAC;
	threshold = _threshold;
}

void BVHBuilder::BuildBVH(Scene & scene) {
	int n = scene.geometries.size();

	/*sort primitives with radix sort*/
	RadixSort::radixsort(scene.geometries, n);

	//for (size_t i = 0; i < n; i++)
	//{
	//	std::cout << i << "\t" <<
	//	scene.geometries[i]->getMortonBits() << std::endl;
	//}
	//std::cout << getPivot(scene.geometries) << std::endl;

	std::vector<std::shared_ptr< Container>> temp;
	if (!isAAC)//not optimized agglomerative clustering
	{
		temp.reserve(n);
		for (int i = 0; i < n; i++)
		{
			temp.push_back((ContainerFactory().CreateContainer(scene.geometries[i], type)));
		}
	}
	else {//using optimized agglomerative clustering
		buildTree(temp, scene.geometries, type);
	}

	//create cluster to root
	CombineCluster(temp, 1);

	//scene only contains root of cluster tree
	scene.bin = temp[0];
}


void BVHBuilder::buildTree(std::vector<std::shared_ptr<Container>>& bins, std::vector<std::shared_ptr<Triangle>>& primitives, Container::TYPE _type)
{
	/*create cluster if primitives number is below threshold*/
	if (primitives.size() < threshold)
	{
		for (int i = 0; i < primitives.size(); i++)
		{
			bins.push_back(ContainerFactory().CreateContainer(primitives[i], _type));
		}
		CombineCluster(bins, f(threshold));
		return;
	}

	/*split primitives into two groups besed on pivot*/
	int pivot = getPivot(primitives);
	std::vector<std::shared_ptr<Triangle>> left(primitives.begin(), primitives.begin() + pivot);// pivot included in left
	std::vector<std::shared_ptr<Triangle>> right(primitives.begin() + pivot, primitives.end());

	/*build left and right tree separately*/
	std::vector<std::shared_ptr< Container>>& leftTree = bins;
	std::vector<std::shared_ptr< Container>> rightTree;

	std::thread lBuild(buildTree, std::ref(leftTree), std::ref(left), _type);
	std::thread rBuild(buildTree, std::ref(rightTree), std::ref(right), _type);

	lBuild.join();
	rBuild.join();

	/*combine two vec and create cluster*/
	std::move(rightTree.begin(), rightTree.end(), std::inserter(bins, bins.end()));

	CombineCluster(bins, f(bins.size()));
}


/*list partition function,
* pivot is the frst bit 'flip'
* ex : [0] 00000111
*      [1] 00001000
*      [2] 00001000
*      [3] 00100000
*      pivot -> 3 (flipped on third element 000xxxxx to 001xxxxx)
*/
int BVHBuilder::getPivot(std::vector<std::shared_ptr<Triangle>>& geo)
{
	for (int i = 0; i < 30; i++)
	{
		for (int j = 1; j < geo.size(); j++)
		{

			//we shift for better performance and bcz morton code is no longer needed;
			auto last = geo[j - 1]->getMortonBits() <<= 1;
			auto curr = geo[j]->getMortonBits() <<= 1;
			if ((curr[0] != last[0]))
				return j;
		}
	}
	return geo.size() / 2;
}

//combine [bins] cluster to [limit] cluster
void BVHBuilder::CombineCluster(std::vector<std::shared_ptr<Container>>& bins, int limit)
{
	/*precalculate bestmatch to be used in next iteration*/
	std::vector<std::thread> t;
	for (int i = 0; i < bins.size(); i++)
	{
		t.push_back(std::thread(ContainerFactory::FindBestMatch, bins[i], bins));
	}
	std::for_each(t.begin(), t.end(), std::mem_fn(&std::thread::join));
	t.clear();

	float bestDist;
	std::shared_ptr< Container> left;
	std::shared_ptr< Container> right;
	while (bins.size() > limit)
	{
		bestDist = INFINITY;
		left = nullptr;
		right = nullptr;

		/*iterate to search best match*/
		for (int i = 0; i < bins.size(); i++)
		{
			if (bins[i] == right)
				continue;
			if (bins[i]->areaWithClosest < bestDist)
			{
				bestDist = bins[i]->areaWithClosest;
				left = bins[i];
				right = bins[i]->closest;
			}
		}

		/*
		delete L and R from bins, push new bin [N]:
		*/
		auto newBin(ContainerFactory().CombineContainer(left, right));
		bins.push_back(newBin);
		auto indexL = std::find(bins.begin(), bins.end(), left);
		std::swap(*indexL, bins.back()); bins.pop_back();
		auto indexR = std::find(bins.begin(), bins.end(), right);
		std::swap(*indexR, bins.back()); bins.pop_back();

		/*change bestmatch of bin if its bestmatch is [L] or [R]*/
		ContainerFactory::FindBestMatch(newBin, bins);
		for (int i = 0; i < bins.size(); i++)
		{
			if (bins[i]->closest == left || bins[i]->closest == right)
				ContainerFactory::FindBestMatch(bins[i], bins);
		}
	}
}

BVHBuilder::~BVHBuilder()
{
}
