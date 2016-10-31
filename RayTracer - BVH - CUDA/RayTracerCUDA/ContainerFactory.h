#pragma once
#include"BoxContainer.h"
#include"SphereContainer.h"
class ContainerFactory
{
public:
	ContainerFactory();
	~ContainerFactory();
	std::shared_ptr<Container> CreateContainer(std::shared_ptr<Geometry> geo, Container::TYPE type = Container::BOX) {
		if (type == Container::SPHERE)
			return std::make_shared<SphereContainer>(SphereContainer(geo));
		else// if (type == Container.TYPE.BOX)
			return std::make_shared<BoxContainer>(BoxContainer(geo));
	}

	std::shared_ptr <Container> CombineContainer(std::shared_ptr <Container> a, std::shared_ptr <Container> b)
	{
		if (a->type != b->type)
			return nullptr;
		if (a->type == Container::SPHERE)
			return std::make_shared<SphereContainer>(SphereContainer(*std::static_pointer_cast<SphereContainer>(a), *std::static_pointer_cast<SphereContainer>(b)));
		else //if (a.Type == Container.TYPE.BOX)
			return std::make_shared<BoxContainer>(BoxContainer(*std::static_pointer_cast<BoxContainer>(a), *std::static_pointer_cast<BoxContainer>(b)));
		return nullptr;
	}
	void FindBestMatch(std::shared_ptr< Container> bin, std::vector<std::shared_ptr< Container>> others)
	{
		float bestDist = INFINITY;
		std::shared_ptr< Container> bestmatch = nullptr;
		for (int i = 0; i < others.size(); i++)
		{
			if (others[i] == bin)
				continue;
			std::shared_ptr< Container>  newBin = ContainerFactory().CombineContainer(bin, others[i]);
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

