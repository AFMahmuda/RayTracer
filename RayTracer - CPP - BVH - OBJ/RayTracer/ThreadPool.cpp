#include "ThreadPool.h"

ctpl::thread_pool ThreadPool::tp(std::thread::hardware_concurrency() - 1);

ThreadPool::ThreadPool()
{
}


ThreadPool::~ThreadPool()
{
}
