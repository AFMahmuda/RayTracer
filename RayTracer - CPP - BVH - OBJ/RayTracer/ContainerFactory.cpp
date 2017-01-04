#include "ContainerFactory.h"

ContainerFactory::ContainerFactory()
{
}

ContainerFactory::~ContainerFactory()
{
}

std::shared_ptr<Container> ContainerFactory::CreateContainer(std::shared_ptr<Triangle> &geo, Container::TYPE type) {
	if (type == Container::TYPE::BOX)
		return std::make_shared<BoxContainer>(BoxContainer(geo));
	if (type == Container::TYPE::SPHERE)
		return std::make_shared<SphereContainer>(SphereContainer(geo));
}

std::shared_ptr<Container> ContainerFactory::combineContainer(std::shared_ptr<Container> &a, std::shared_ptr<Container> &b)
{
	if (a->type != b->type)
		return nullptr;
	if (a->type == Container::SPHERE)
		return std::make_shared<SphereContainer>(std::static_pointer_cast<SphereContainer>(a), std::static_pointer_cast<SphereContainer>(b));
	else //if (a.Type == Container.TYPE.BOX)
		return std::make_shared<BoxContainer>(std::static_pointer_cast<BoxContainer>(a), std::static_pointer_cast<BoxContainer>(b));
	return nullptr;
}

void ContainerFactory::findBestMatch(std::shared_ptr<Container>& bin, std::vector<std::shared_ptr<Container>>& others)
{

	float bestDist = INFINITY;
	std::shared_ptr< Container> bestmatch = nullptr;
	for (int i = 0; i < others.size(); i++)
	{
		if (others[i] == bin)
			continue;


		std::vector<std::shared_ptr <Container>>::iterator temp = std::find((bin)->calculatedPair.begin(), (bin)->calculatedPair.end(), others.at(i));
		int index = temp - bin->calculatedPair.begin();

		if (index != bin->calculatedPair.size())
		{
			if ((bin)->calculatedPairArea[index] < bestDist)
			{
				bestDist = (bin)->calculatedPairArea[index];
				bestmatch = others[i];
			}
			continue;
		}

		std::shared_ptr< Container>  newBin = ContainerFactory().combineContainer(bin, others[i]);
		if (newBin->area < bestDist)
		{
			bestDist = newBin->area;
			bestmatch = others[i];
		}
		(bin)->calculatedPair.push_back(others[i]);
		(bin)->calculatedPairArea.push_back(newBin->area);


	}
	bin->closest = bestmatch;
	bin->areaWithClosest = bestDist;
}
