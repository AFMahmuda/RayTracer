#pragma once
#include"BoxContainer.h"
#include"SphereContainer.h"
class ContainerFactory
{
public:
	ContainerFactory();
	~ContainerFactory();
	std::shared_ptr<Container> CreateContainer(std::shared_ptr<Triangle>& geo, Container::TYPE type = Container::BOX);
	std::shared_ptr <Container> combineContainer(std::shared_ptr <Container>& a, std::shared_ptr <Container>& b);
	void findBestMatch(std::shared_ptr< Container>& bin, std::vector<std::shared_ptr< Container>>& others);
};

