#pragma once
#include<memory>
#include<vector>

#include"Geometry.h"


//http://www.sanfoundry.com/cpp-program-implement-radix-sort/
class RadixSort
{
private:
	static int getMax(std::vector<std::shared_ptr< Geometry>> arr, int n);
	/*count sort of arr[]	*/
	static void countSort(std::vector<std::shared_ptr< Geometry>> arr, int n, int exp);
public:
	RadixSort();
	/*sorts arr[] of size n using Radix Sort	*/
	static void radixsort(std::vector<std::shared_ptr< Geometry>> arr, int n);
	~RadixSort();
};

