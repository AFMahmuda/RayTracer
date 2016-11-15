#include "RadixSort.h"



int RadixSort::getMax(std::vector<std::shared_ptr<Triangle>>& arr, int n)
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

void RadixSort::countSort(std::vector<std::shared_ptr<Triangle>>& arr, int n, int exp)
{
	std::vector<std::shared_ptr< Triangle>> output;
	output.resize(n);
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

RadixSort::RadixSort()
{




}



/*sorts arr[] of size n using Radix Sort	*/

void RadixSort::radixsort(std::vector<std::shared_ptr<Triangle>>& arr, int n)
{
	if (n > 0)
	{
		int m = getMax(arr, n);
		for (int exp = 1; m / exp > 0; exp *= 10)
			countSort(arr, n, exp);
	}
}

RadixSort::~RadixSort()
{
}
