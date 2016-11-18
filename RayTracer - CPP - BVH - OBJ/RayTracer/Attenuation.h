#pragma once
class Attenuation
{
public:
	float cons, line, quad;
	Attenuation() :cons(0), line(0), quad(0) {	}
	Attenuation(float * param) :cons(param[0]), line(param[1]), quad(param[2]) {	}
	~Attenuation();
};

