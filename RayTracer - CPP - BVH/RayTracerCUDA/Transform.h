#pragma once
#include"Matrix.h"
class Transform
{


public:
	Matrix matrix;

	Transform() :matrix(Matrix(4, 4)) {	}
	Transform(const Transform& orig) :matrix(orig.matrix) {	}

	~Transform();
};

