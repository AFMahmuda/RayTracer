#pragma once
#include"BoxContainer.h"
#include"SphereContainer.h"
#include"ContainerFactory.h"
#include"RadixSort.h"
#include"Scene.h"
#include <vector>

#include<memory>
using namespace std;
class BVHBuilder
{
public:
	bool isAAC;
	Container::TYPE type;

	BVHBuilder(Container::TYPE _type = Container::BOX, bool _isAAC = true) {
		type = _type;
		isAAC = _isAAC;
	}

	void BuildBVH(Scene& scene) {
		int n = scene.geometries.size();
		for (int i = 0; i < n; i++)
			scene.geometries[i]->getMortonPos();

		RadixSort::radixsort(scene.geometries, n);
		std::vector<shared_ptr< Container>> temp;
		if (!isAAC)
		{
			for (int i = 0; i < n; i++)
			{

				temp.push_back((ContainerFactory().CreateContainer(scene.geometries[i], type)));
			}
		}
		else {
			temp = BuildTree(scene.geometries, n);
		}




		temp = CombineCluster(temp, 1);
		scene.container = temp[0];
	}

	int threshold = 4; //4 or 20
	std::vector<shared_ptr< Container>> BuildTree(vector<	shared_ptr< Geometry>> primitives, int n)
	{
		//List<Container> bins = new List<Container>();
		//if (primitives.Length < threshold)
		//{

		//	for (int i = 0; i < primitives.Length; i++)
		//	{
		//		bins.Add(ContainerFactory.Instance.CreateContainer(primitives[i], type));
		//	}
		//	return CombineCluster(bins, f(threshold));
		//}

		//int pivot = getPivot(primitives);
		//Geometry[] left = primitives.Take(pivot).ToArray();
		//Geometry[] right = primitives.Skip(pivot).ToArray(); // pivot included in right

		//List<Container> leftTree = new List<Container>(), rightTree = new List<Container>();

		//Task[] BuildTreeTask = new Task[2];
		//BuildTreeTask[0] = Task.Factory.StartNew(() = > leftTree = BuildTree(left).ToList());
		//BuildTreeTask[1] = Task.Factory.StartNew(() = > rightTree = BuildTree(right).ToList());
		//Task.WaitAll(BuildTreeTask);


		//bins.AddRange(leftTree);
		//bins.AddRange(rightTree);
		////bins.AddRange(BuildTree(left));
		////bins.AddRange(BuildTree(right));
		//return CombineCluster(bins, f(bins.Count));
		return std::vector<shared_ptr< Container>>();
	}

	/*cluster reduction function
	* n -> number of input clusters
	* f(n) -> number of max output cluster
	*/

	int f(int n)
	{
		return n / 2;
	}

	/*list partition function,
	* pivot is the frst bit 'flip'
	* ex : [0] 00000111
	*      [1] 00001000
	*      [2] 00001000
	*      [3] 00100000
	*      pivot -> 3 (flipped on third element 000xxxxx to 001xxxxx)
	*/
	int getPivot(Geometry** geo, int n)
	{
		//for (int i = 0; i < 30; i++)
		//{
		//	for (int j = 1; j < geo.Length; j++)
		//	{
		//		string last = geo[j - 1].GetMortonBitString();
		//		string curr = geo[j].GetMortonBitString();
		//		if (curr[i] != last[i])
		//			return j;
		//	}
		//}
		//return geo.Length / 2;
	}


	//combine bins cluster to [limit] cluster
	std::vector<shared_ptr< Container>> CombineCluster(vector<shared_ptr< Container>> bins, int limit)
	{
		//Console.WriteLine("Combining from " + bins.Count + " to " + limit);

		for (int i = 0; i < bins.size(); i++)
		{
			ContainerFactory().FindBestMatch(bins[i], bins);

		}

		while (bins.size() > limit)
		{
			float bestDist = INFINITY;
			shared_ptr< Container> left = nullptr;
			shared_ptr< Container> right = nullptr;

			std::vector<std::shared_ptr <Container>>::iterator indexR, indexL;
			for (int i = 0; i < bins.size(); i++)
			{
				if (bins[i]->areaWithClosest < bestDist)
				{
					bestDist = bins[i]->areaWithClosest;
					left = bins[i];
					right = bins[i]->closest;

				}
			}

			shared_ptr< Container> newBin = ContainerFactory().CombineContainer(left, right);
			newBin->LChild = left;
			newBin->RChild = right;
			bins.push_back(newBin);



			indexL = std::find(bins.begin(), bins.end(), right);
			bins.erase(indexL);

			indexR = std::find(bins.begin(), bins.end(), left);
			bins.erase(indexR);

			ContainerFactory().FindBestMatch(newBin, bins);
			for (int i = 0; i < bins.size(); i++)
			{
				if (bins[i]->closest == left || bins[i]->closest == right)
					ContainerFactory().FindBestMatch(bins[i], bins);
			}
		}

		return bins;
	}
	~BVHBuilder();
};

