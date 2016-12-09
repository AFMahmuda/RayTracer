#include "BVHBuilder.h"

#include"RadixSort.h"
#include"ThreadPool.h"

int BVHBuilder::threshold = 20;

BVHBuilder::BVHBuilder(Container::TYPE _type, bool _isAAC, int _threshold) {
	type = _type;
	isAAC = _isAAC;
	threshold = _threshold;

}

std::shared_ptr<Container> BVHBuilder::BuildBVH(std::vector<std::shared_ptr<Triangle>>& primitives) {
	int n = primitives.size();

	//nothing in scene
	if (n == 0)
		return nullptr;

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
			temp.push_back((ContainerFactory().CreateContainer(primitives[i], type)));
		}
	}
	else {//using optimized agglomerative clustering
		if (ThreadPool::tp.n_idle() > 0) {
			std::future<void> buildtree = ThreadPool::tp.push(buildTree, std::ref(temp), std::ref(primitives), type);
			buildtree.get();
		}
		else
			buildTree(0, temp, primitives, type);
	}

	//create complete tree
	CombineCluster(temp, 1);

	//only need root of cluster tree
	return temp[0];
}

/*
input
bins,
p = primitives
type

output
modified bins
*/
void BVHBuilder::buildTree(int id, std::vector<std::shared_ptr<Container>>& bins, std::vector<std::shared_ptr<Triangle>>& primitives, Container::TYPE _type)
{
	//create clusters if primitives number < threshold
	if (primitives.size() < threshold)
	{
		for (int i = 0; i < primitives.size(); i++)
		{
			bins.push_back(ContainerFactory().CreateContainer(primitives[i], _type));
		}
		CombineCluster(bins, f(threshold));
		return;
	}

	//split primitives into two groups besed on pivot
	int pivot = getPivot(primitives);
	std::vector<std::shared_ptr<Triangle>> lPrimitives(primitives.begin(), primitives.begin() + pivot);// pivot included in left
	std::vector<std::shared_ptr<Triangle>> rPrimitives(primitives.begin() + pivot, primitives.end());

	//build left and right tree separately
	std::vector<std::shared_ptr< Container>>& lTree = bins;
	std::vector<std::shared_ptr< Container>> rTree;

	//if there's an indle thread in pool, push new thread.
	std::future<void> lBuild;
	switch (ThreadPool::tp.n_idle() > 0)
	{
	case true:
		lBuild = ThreadPool::tp.push(buildTree, std::ref(lTree), std::ref(lPrimitives), _type);
		buildTree(id, rTree, rPrimitives, _type);
		lBuild.get();
		break;
	case false:
		buildTree(id, lTree, lPrimitives, _type);
		buildTree(id, rTree, rPrimitives, _type);
		break;
	}

	/*combine two vec and than create clusters*/
	std::move(rTree.begin(), rTree.end(), std::inserter(bins, bins.end()));
	CombineCluster(bins, f(bins.size()));
	return;
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
	/*precalculate bestmatch to be used in iteration*/
	for (int i = 0; i < bins.size(); i++)
	{
		ContainerFactory::FindBestMatch(bins[i], bins);
	}

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
