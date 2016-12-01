#include "RadixSort.h"


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

void RadixSort::radixsort(std::vector<std::shared_ptr<Triangle>>& arr)
{
	int n = arr.size();
	if (n > 0)
	{
		int m = 0x3FFFFFFF;
		for (int exp = 1; m / exp > 0; exp *= 10)
			countSort(arr, n, exp);
	}
}

RadixSort::~RadixSort()
{
}
