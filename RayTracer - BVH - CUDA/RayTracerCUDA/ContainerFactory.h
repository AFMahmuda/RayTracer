#pragma once
#include"BoxContainer.h"
#include"SphereContainer.h"
class ContainerFactory
{
public:
	ContainerFactory();
	~ContainerFactory();
	Container * CreateContainer(Geometry* geo, Container::TYPE type = Container::BOX) {
		if (type == Container::SPHERE)
			return new SphereContainer(geo);
		else// if (type == Container.TYPE.BOX)
			return new BoxContainer(geo);
	}

	Container* CombineContainer(Container* a, Container* b)
	{
		if (a->type != b->type)
			return nullptr;
		if (a->type == Container::SPHERE)
			return new SphereContainer((SphereContainer)*(SphereContainer*)a, (SphereContainer)*(SphereContainer*)b);
		else //if (a.Type == Container.TYPE.BOX)
			return new BoxContainer((BoxContainer)*(BoxContainer*)a, (BoxContainer)*(BoxContainer*)b);
	}
	void FindBestMatch(Container* bin,std::vector<Container*> others)
	{
		float bestDist = INFINITY;
		Container* bestmatch = nullptr;
		for (int i = 0; i < others.size(); i++)
		{
			if (others[i] == bin)
				continue;
			Container* newBin = ContainerFactory().CombineContainer(bin, others[i]);
			if (newBin->area < bestDist)
			{
				bestDist = newBin->area;
				bestmatch = others[i];
			}
		}
		bin->closest = bestmatch;
		bin->areaWithClosest = bestDist;
	}
};

