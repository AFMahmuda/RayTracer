#pragma once
class Attenuation
{
public:
	float cons, line, quad;
	Attenuation() :Attenuation(new float[3]{ 0,0,0 }) {	}
	Attenuation(float * param) :cons(param[1]), line(param[1]), quad(param[2]) {}


	~Attenuation();
};

