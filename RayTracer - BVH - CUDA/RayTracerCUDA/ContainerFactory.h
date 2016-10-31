#pragma once
#include"BoxContainer.h"
#include"SphereContainer.h"
class ContainerFactory
{
public:
	ContainerFactory();
	~ContainerFactory();
	shared_ptr<Container> CreateContainer(shared_ptr<Geometry> geo, Container::TYPE type = Container::BOX) {
		if (type == Container::SPHERE)
			return make_shared<SphereContainer>(SphereContainer(geo));
		else// if (type == Container.TYPE.BOX)
			return make_shared<BoxContainer>(BoxContainer(geo));
	}

	shared_ptr <Container> CombineContainer(shared_ptr <Container> a, shared_ptr <Container> b)
	{
		if (a->type != b->type)
			return nullptr;
		if (a->type == Container::SPHERE)
			return make_shared<SphereContainer>(SphereContainer(*static_pointer_cast<SphereContainer>(a), *static_pointer_cast<SphereContainer>(b)));
		else //if (a.Type == Container.TYPE.BOX)
			return make_shared<BoxContainer>(BoxContainer(*static_pointer_cast<BoxContainer>(a), *static_pointer_cast<BoxContainer>(b)));
		return nullptr;
	}
	void FindBestMatch(shared_ptr< Container> bin, std::vector<shared_ptr< Container>> others)
	{
		float bestDist = INFINITY;
		shared_ptr< Container> bestmatch = nullptr;
		for (int i = 0; i < others.size(); i++)
		{
			if (others[i] == bin)
				continue;
			shared_ptr< Container>  newBin = ContainerFactory().CombineContainer(bin, others[i]);
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

