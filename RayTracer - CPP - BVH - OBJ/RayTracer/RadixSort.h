#pragma once
#include<memory>
#include<vector>
#include"Triangle.h"


//http://www.sanfoundry.com/cpp-program-implement-radix-sort/
class RadixSort
{
private:
	/*count sort of arr[]*/
	static void countSort(int id,std::vector<std::shared_ptr<Triangle>>& arr, int start, int end, int step);

public:
	RadixSort();
	/*sorts arr[] of size n using Radix Sort*/
	static void radixsort(std::vector<std::shared_ptr< Triangle>>& arr, int n);
	~RadixSort();
};

