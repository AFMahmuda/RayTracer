#pragma once
class Attenuation
{
public:
	float cons, line, quad;
	Attenuation() :Attenuation(new float[3]{ 1,0,0 }) {	}
	Attenuation(float * param) :cons(param[0]), line(param[1]), quad(param[2]) {}


	~Attenuation();
};

