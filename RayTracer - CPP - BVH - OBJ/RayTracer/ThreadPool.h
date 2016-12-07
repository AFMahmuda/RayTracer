#pragma once
#include "ctpl_stl.h"
class ThreadPool
{
public:
	static ctpl::thread_pool tp;
	ThreadPool();
	~ThreadPool();
};

