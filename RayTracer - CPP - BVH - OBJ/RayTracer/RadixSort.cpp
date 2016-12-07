#include "RadixSort.h"
#include <utility>      // std::move
#include <iostream>
#include "ThreadPool.h"

/*count sort of arr[]	*/
void RadixSort::countSort(int id, std::vector<std::shared_ptr<Triangle>>& arr, int start, int end, int step) {

	if (step < 0 || (end - start) + 1 <= 1)
		return;

	std::vector<std::shared_ptr<Triangle>> output;
	output.resize(end - start + 1);
	int bin[2] = { 0,0 };
	for (size_t i = start; i <= end; i++)
	{
		bin[arr[i]->getMortonBits()[step]]++;
	}

	bin[1] += bin[0];
	int mid = bin[0] + start;
	for (int i = end; i >= start; i--)
	{
		//std::cout << step << " " << start << " " << end << " " << (end - start) << " " << i << " " << bin[arr[i]->getMortonBits()[step]] << " " << bin[arr[i]->getMortonBits()[step]] - 1;
		output[bin[arr[i]->getMortonBits()[step]] - 1] = arr[i];
		bin[arr[i]->getMortonBits()[step]]--;
		//std::cout << " done" << "\n";
	}
	for (int i = start; i <= end; i++)
	{
		arr[i] = output[i - start];
	}

	output.clear();
	if (ThreadPool::tp.n_idle() > 0)
	{
		auto leftSort = ThreadPool::tp.push(countSort, std::ref(arr), start, mid - 1, step - 1);
		countSort(id, arr, mid, end, step - 1);
		leftSort.get();
	}
	else
	{
		countSort(id, arr, start, mid - 1, step - 1);
		countSort(id, arr, mid, end, step - 1);
	}

}

RadixSort::RadixSort()
{

}


/*sorts arr[] of size n using Radix Sort	*/
void RadixSort::radixsort(std::vector<std::shared_ptr<Triangle>>& arr, int n)
{
	if (n > 0)
	{
		countSort(0, arr, 0, n - 1, 29);
	}
}

RadixSort::~RadixSort()
{
}
