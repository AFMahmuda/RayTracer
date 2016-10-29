#pragma once
#include<vector>
#include"Geometry.h"

class Container
{
public:
	enum TYPE {
		BOX, SPHERE
	};

	Container* childs;
	Geometry* geo;
	float area;

	virtual bool IsIntersecting(Ray ray) { return false; }

	float areaWithClosest = FLT_MAX;
	Container* closest = nullptr;

	void FindBestMatch(std::vector<Container*> bins)
	{
		float bestDist = FLT_MAX;
		Container* bestmatch = nullptr;
		for (int i = 0; i < bins.size(); i++)
		{
			if (bins[i] == this)
				continue;
			//Container newBin = ContainerFactory.Instance.CombineContainer(this, bins[i]);
			//if (newBin.area < bestDist)
			//{
			//	bestDist = newBin.area;
			//	bestmatch = bins[i];
			//}
		}
		closest = bestmatch;
		areaWithClosest = bestDist;
	}

	Container();
	~Container();
};

