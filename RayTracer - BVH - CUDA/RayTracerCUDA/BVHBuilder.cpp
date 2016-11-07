#include "BVHBuilder.h"




BVHBuilder::BVHBuilder(Container::TYPE _type, bool _isAAC) {
	type = _type;
	isAAC = _isAAC;
}

void BVHBuilder::BuildBVH(Scene & scene) {
	int n = scene.geometries.size();

	/*precalculate mortoncode*/
	for (rsize_t i = 0; i < n; i++)
		scene.geometries[i]->getMortonPos();

	/*sort primitives with radix sort*/
	RadixSort::radixsort(scene.geometries, n);


	std::vector<std::shared_ptr< Container>> temp;
	if (!isAAC)//not optimized agglomerative clustering
	{
		for (int i = 0; i < n; i++)
		{
			temp.push_back((ContainerFactory().CreateContainer(scene.geometries[i], type)));
		}
	}
	else {//using optimized agglomerative clustering
		temp = BuildTree(scene.geometries);
	}

	//create cluster
	temp = CombineCluster(temp, 1);

	//scene only contains root of cluster tree
	scene.bin = temp[0];
}

//4 or 20

std::vector<std::shared_ptr<Container>> BVHBuilder::BuildTree(std::vector<std::shared_ptr<Triangle>> primitives)
{
	/*create cluster if primitives number is below threshold*/
	if (primitives.size() < threshold)
	{
		std::vector<std::shared_ptr< Container>> bins;

		for (int i = 0; i < primitives.size(); i++)
		{
			bins.push_back(ContainerFactory().CreateContainer(primitives[i], type));
		}
		return CombineCluster(bins, f(threshold));
	}


	/*split primitives into two groups besed on pivot*/
	int pivot = getPivot(primitives);
	std::vector<std::shared_ptr<Triangle>> left(primitives.begin(), primitives.begin() + pivot);
	std::vector<std::shared_ptr<Triangle>> right(primitives.begin() + pivot, primitives.end());// pivot included in right

	/*build left and right tree separately*/
	std::vector<std::shared_ptr< Container>> leftTree = BuildTree(left);
	std::vector<std::shared_ptr< Container>> rightTree = BuildTree(right);



	/*combine two vec and create cluster*/
	leftTree.insert(leftTree.end(), rightTree.begin(), rightTree.end());
	return CombineCluster(leftTree, f(leftTree.size()));
}

/*list partition function,
* pivot is the frst bit 'flip'
* ex : [0] 00000111
*      [1] 00001000
*      [2] 00001000
*      [3] 00100000
*      pivot -> 3 (flipped on third element 000xxxxx to 001xxxxx)
*/

int BVHBuilder::getPivot(std::vector<std::shared_ptr<Triangle>> geo)
{
	for (int i = 0; i < 30; i++)
	{
		for (int j = 1; j < geo.size(); j++)
		{
			std::string last = geo[j - 1]->getMortonBitString();
			std::string curr = geo[j]->getMortonBitString();
			if (curr[i] != last[i])
				return j;
		}
	}
	return geo.size() / 2;
}


//combine [bins] cluster to [limit] cluster

std::vector<std::shared_ptr<Container>> BVHBuilder::CombineCluster(std::vector<std::shared_ptr<Container>> bins, int limit)
{
	/*precalculate bestmatch to be used in next iteration*/
	for (int i = 0; i < bins.size(); i++)
	{
		ContainerFactory().FindBestMatch(bins[i], bins);
	}

	while (bins.size() > limit)
	{
		float bestDist = INFINITY;
		std::shared_ptr< Container> left = nullptr;
		std::shared_ptr< Container> right = nullptr;

		/*iterate to search best match*/
		for (int i = 0; i < bins.size(); i++)
		{
			if (bins[i]->areaWithClosest < bestDist)
			{
				bestDist = bins[i]->areaWithClosest;
				left = bins[i];
				right = bins[i]->closest;
			}
		}

		/*
		delete L and R from bins, push new bin [N]:
		swap [L] and [last]	:: xx[L][R]xxxx
		add [N]				:: xxx[R]xxx[L]
		swap [R] and [last]	:: xxx[R]xxx[L][N]
		pop [last] twice	:: xxx[N]xxx[L][R]
		:: xxx[N]xxx
		*/
		std::shared_ptr< Container> newBin = ContainerFactory().CombineContainer(left, right);
		std::vector<std::shared_ptr <Container>>::iterator indexL = std::find(bins.begin(), bins.end(), left);
		std::swap(*indexL, bins.back());
		bins.push_back(newBin);
		std::vector<std::shared_ptr <Container>>::iterator indexR = std::find(bins.begin(), bins.end(), right);
		std::swap(*indexR, bins.back());
		bins.pop_back();
		bins.pop_back();

		/*
		change bestmatch of bin if its bestmatch is [l] or [r]
		*/
		ContainerFactory().FindBestMatch(newBin, bins);
		for (int i = 0; i < bins.size(); i++)
		{
			if (bins[i]->closest == left || bins[i]->closest == right)
				ContainerFactory().FindBestMatch(bins[i], bins);
		}
	}

	return bins;
}

BVHBuilder::~BVHBuilder()
{
}
