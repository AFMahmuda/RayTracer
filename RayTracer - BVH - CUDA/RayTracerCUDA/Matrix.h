#pragma once
#include "Vec3.h"
#include <stdio.h>

class Matrix
{

private:
	int rowNum;
	int colNum;
	float* vals;

	bool hasIdentity;

	Matrix* identity;
	void CreateIdentity();

	bool hasInverse;
	Matrix* inverse;
	void CreateInverse();
public:

	Matrix(int row, int col);
	Matrix(int row, int col, float *val);

	void Setvalue(float* vals);
	Matrix& GetRow(int row);

	Matrix& GetCol(int col);

	Matrix& operator*(float s) {
		Matrix newMat = Matrix(rowNum, colNum);

		for (int row = 0; row < rowNum; row++)
		{
			for (int col = 0; col < colNum; col++)
			{
				float val = (vals[row *colNum + col] * s);
				newMat(row, col) = (val);
			}
		}
		return newMat;
	}
	Matrix& Identity();
	Matrix& Inverse();
	static Vec3 Mul44x41(Matrix& m, Vec3& v);
	static Matrix Mul44x44(Matrix& matA, Matrix& matB);
	static Matrix Mul44x41(Matrix& mat44, Matrix& mat41);
	float& operator()(int r, int c) {
		return vals[r*colNum + c];
	}
	float& ItemAt(int r, int c) {
		return vals[r*colNum + c];
	}

	~Matrix();
};

