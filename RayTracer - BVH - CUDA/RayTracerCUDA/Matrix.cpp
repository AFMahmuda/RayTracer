#include "Matrix.h"



Matrix::~Matrix()
{
}

Matrix::Matrix(int row, int col) {
	colNum = col;
	rowNum = row;

	vals = new float*[row];
	for (int i = 0; i < row; i++)
	{
		vals[i] = new float[col];
	}

	for (int i = 0; i < rowNum; i++)
	{
		for (int j = 0; j < colNum; j++)
		{
			Matrix(*this)(i, j) = 0;
		}
	}
	hasIdentity = false;
	hasInverse = false;
}

Matrix::Matrix(int row, int col, float * val) :Matrix(row, col) {
	Setvalue(val);
}
void Matrix::CreateIdentity() {

	identity = new Matrix(rowNum, colNum);
	for (int i = 0; i < rowNum; i++)
	{
		for (int j = 0; j < colNum; j++)
		{
			identity->ItemAt(i, j) = 0;
		}
		identity->ItemAt(i, i) = 1;
	}
}

void Matrix::CreateInverse() {
	inverse = new Matrix(rowNum, colNum);

	float s0 = vals[0][0] * vals[1][1] - vals[1][0] * vals[0][1];
	float s1 = vals[0][0] * vals[1][2] - vals[1][0] * vals[0][2];
	float s2 = vals[0][0] * vals[1][3] - vals[1][0] * vals[0][3];
	float s3 = vals[0][1] * vals[1][2] - vals[1][1] * vals[0][2];
	float s4 = vals[0][1] * vals[1][3] - vals[1][1] * vals[0][3];
	float s5 = vals[0][2] * vals[1][3] - vals[1][2] * vals[0][3];

	float c5 = vals[2][2] * vals[3][3] - vals[3][2] * vals[2][3];
	float c4 = vals[2][1] * vals[3][3] - vals[3][1] * vals[2][3];
	float c3 = vals[2][1] * vals[3][2] - vals[3][1] * vals[2][2];
	float c2 = vals[2][0] * vals[3][3] - vals[3][0] * vals[2][3];
	float c1 = vals[2][0] * vals[3][2] - vals[3][0] * vals[2][2];
	float c0 = vals[2][0] * vals[3][1] - vals[3][0] * vals[2][1];

	float invdet = 1.f / (s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0);

	Matrix(*inverse)(0, 0) = (vals[1][1] * c5 - vals[1][2] * c4 + vals[1][3] * c3) * invdet;
	Matrix(*inverse)(0, 1) = (-vals[0][1] * c5 + vals[0][2] * c4 - vals[0][3] * c3) * invdet;
	Matrix(*inverse)(0, 2) = (vals[3][1] * s5 - vals[3][2] * s4 + vals[3][3] * s3) * invdet;
	Matrix(*inverse)(0, 3) = (-vals[2][1] * s5 + vals[2][2] * s4 - vals[2][3] * s3) * invdet;

	Matrix(*inverse)(1, 0) = (-vals[1][0] * c5 + vals[1][2] * c2 - vals[1][3] * c1) * invdet;
	Matrix(*inverse)(1, 1) = (vals[0][0] * c5 - vals[0][2] * c2 + vals[0][3] * c1) * invdet;
	Matrix(*inverse)(1, 2) = (-vals[3][0] * s5 + vals[3][2] * s2 - vals[3][3] * s1) * invdet;
	Matrix(*inverse)(1, 3) = (vals[2][0] * s5 - vals[2][2] * s2 + vals[2][3] * s1) * invdet;

	Matrix(*inverse)(2, 0) = (vals[1][0] * c4 - vals[1][1] * c2 + vals[1][3] * c0) * invdet;
	Matrix(*inverse)(2, 1) = (-vals[0][0] * c4 + vals[0][1] * c2 - vals[0][3] * c0) * invdet;
	Matrix(*inverse)(2, 2) = (vals[3][0] * s4 - vals[3][1] * s2 + vals[3][3] * s0) * invdet;
	Matrix(*inverse)(2, 3) = (-vals[2][0] * s4 + vals[2][1] * s2 - vals[2][3] * s0) * invdet;

	Matrix(*inverse)(3, 0) = (-vals[1][0] * c3 + vals[1][1] * c1 - vals[1][2] * c0) * invdet;
	Matrix(*inverse)(3, 1) = (vals[0][0] * c3 - vals[0][1] * c1 + vals[0][2] * c0) * invdet;
	Matrix(*inverse)(3, 2) = (-vals[3][0] * s3 + vals[3][1] * s1 - vals[3][2] * s0) * invdet;
	Matrix(*inverse)(3, 3) = (vals[2][0] * s3 - vals[2][1] * s1 + vals[2][2] * s0) * invdet;

}



void Matrix::Setvalue(float * vals) {
	int counter = 0;
	for (int row = 0; row < rowNum; row++)
	{
		for (int col = 0; col < colNum; col++)
		{
			this->vals[row][col] = vals[counter++];
		}
	}
	hasInverse = false;
}

Matrix & Matrix::GetRow(int row) {
	Matrix res = Matrix(1, colNum);
	for (int i = 0; i < colNum; i++)
	{
		res(0, i) = vals[row][i];
	}
	return res;
}

Matrix & Matrix::GetCol(int col) {
	Matrix res = Matrix(rowNum, 1);
	for (int i = 0; i < rowNum; i++)
	{
		res(i, 0) = vals[i][col];
	}
	return res;
}

Matrix & Matrix::Identity() {
	if (!hasIdentity)
	{
		CreateIdentity();
		hasIdentity = true;
	}

	return Matrix(*identity);

}

Matrix & Matrix::Inverse() {
	if (!hasInverse) {
		CreateInverse();
		hasInverse = true;
	}
	return Matrix(*inverse);
}

Data3D Matrix::Mul44x41(Matrix & m, Data3D & v) {
	float newX = m(0, 0) * v[0] + m(0, 1) * v[1] + m(0, 2) * v[2] + m(0, 3) * v[3];
	float newY = m(1, 0) * v[0] + m(1, 1) * v[1] + m(1, 2) * v[2] + m(1, 3) * v[3];
	float newZ = m(2, 0) * v[0] + m(2, 1) * v[1] + m(2, 2) * v[2] + m(2, 3) * v[3];

	if (v.h == 0) {
		return Point3(newX, newY, newZ);
	}
	return Vec3(newX, newY, newZ);

}

Matrix Matrix::Mul44x44(Matrix & matA, Matrix & matB) {
	Matrix res = Matrix(4, 4);
	Matrix mat41 = Matrix(4, 1);
	for (int col = 0; col < 4; col++)
	{
		mat41 = matB.GetCol(col);
		mat41 = Mul44x41(matA, mat41);
		for (int row = 0; row < 4; row++)
			res(row, col) = mat41(row, 0);
	}
	return res;

}

Matrix Matrix::Mul44x41(Matrix & mat44, Matrix & mat41) {
	Matrix& res = Matrix(4, 1);
	for (int row = 0; row < 4; row++)
	{
		float val = 0;
		for (int col = 0; col < 4; col++)
			val += (mat44(row, col) * mat41(col, 0));
		res(row, 0) = val;
	}
	return res;

}
