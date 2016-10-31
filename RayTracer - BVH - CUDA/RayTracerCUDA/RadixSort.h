#pragma once
#include"Geometry.h"
#include<memory>
#include<vector>
using namespace std;

//http://www.sanfoundry.com/cpp-program-implement-radix-sort/
class RadixSort
{
private:
	static int getMax(std::vector<shared_ptr< Geometry>> arr, int n)
	{
		unsigned int max = arr[0].get()->getMortonPos();
		for (int i = 1; i < n; i++)
			if (arr[i].get()->getMortonPos() > max)
				max = arr[i].get()->getMortonPos();
		return max;
	}
	/*
	* count sort of arr[]
	*/
	static void countSort(std::vector<shared_ptr< Geometry>> arr, int n, int exp)
	{
		vector<shared_ptr< Geometry>> output;
		output.resize (n);
		int i, count[10] = { 0 };
		for (i = 0; i < n; i++)
			count[(arr[i].get()->getMortonPos() / exp) % 10]++;
		for (i = 1; i < 10; i++)
			count[i] += count[i - 1];
		for (i = n - 1; i >= 0; i--)
		{
			output[count[(arr[i].get()->getMortonPos() / exp) % 10] - 1] = arr[i];
			count[(arr[i].get()->getMortonPos() / exp) % 10]--;
		}
		for (i = 0; i < n; i++)
			arr[i] = output[i];
	}
public:
	RadixSort();

	/*
	* sorts arr[] of size n using Radix Sort
	*/
	static void radixsort(std::vector<shared_ptr< Geometry>> arr, int n)
	{
		int m = getMax(arr, n);
		for (int exp = 1; m / exp > 0; exp *= 10)
			countSort(arr, n, exp);
	}

	~RadixSort();
};

