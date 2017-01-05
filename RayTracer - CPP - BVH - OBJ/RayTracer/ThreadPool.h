#pragma once
#include "ctpl_stl.h"
class ThreadPool
{
public:
	static ctpl::thread_pool tp;	
	static void setMaxThread(int num) {
		tp.resize(num);
	}


	ThreadPool();
	~ThreadPool();
};

