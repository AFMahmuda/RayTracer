#pragma once
#include"Geometry.h"


//http://www.sanfoundry.com/cpp-program-implement-radix-sort/
class RadixSort
{
private:
	static int getMax(Geometry** arr, int n)
	{
		unsigned int max = arr[0]->getMortonPos();
		for (int i = 1; i < n; i++)
			if (arr[i]->getMortonPos() > max)
				max = arr[i]->getMortonPos();
		return max;
	}
	/*
	* count sort of arr[]
	*/
	static void countSort(Geometry** arr, int n, int exp)
	{
		Geometry  **output;
		output = (Geometry**)malloc(sizeof(Geometry*) * n);
		int i, count[10] = { 0 };
		for (i = 0; i < n; i++)
			count[(arr[i]->getMortonPos() / exp) % 10]++;
		for (i = 1; i < 10; i++)
			count[i] += count[i - 1];
		for (i = n - 1; i >= 0; i--)
		{
			output[count[(arr[i]->getMortonPos() / exp) % 10] - 1] = arr[i];
			count[(arr[i]->getMortonPos() / exp) % 10]--;
		}
		for (i = 0; i < n; i++)
			arr[i] = output[i];
	}
public:
	RadixSort();

	/*
	* sorts arr[] of size n using Radix Sort
	*/
	static void radixsort(Geometry** arr, int n)
	{
		int m = getMax(arr, n);
		for (int exp = 1; m / exp > 0; exp *= 10)
			countSort(arr, n, exp);
	}

	~RadixSort();
};

