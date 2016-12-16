#include <fstream>  //file 
#include <iostream>
#include <algorithm> //replace
#include <string>
#include <thread>
#include"TraceManager.h"
#include "ThreadPool.h"
#include"Container.h"

using namespace std;

int main(int argc, char *argv[])
{
  //scene file name, default = "default.scene"  
  string filename = (argc >= 2) ? argv[1] : "scene_default.scene";
  std::replace(filename.begin(), filename.end(), '\\', '/');

  //using aac algorithm (1 = yes / 0 = no ), default = yes;
  bool isAAC = (argc >= 3) ? (atoi(argv[2]) == 1) ? true : false : true;

  //bin type (b = box / s = sphere ), default = box;
  Container::TYPE binType = (argc >= 4) ? (argv[3][0] == 'b') ? Container::BOX : Container::SPHERE : Container::BOX;

  //aac treshold, def=20;
  int thres = (argc >= 5) ? (atoi(argv[4])) : 20;


  //thread number for aac and 
  (argc >= 6) ? ThreadPool::setMaxThread((atoi(argv[5]) - 1)) : ThreadPool::setMaxThread(std::thread::hardware_concurrency() - 1);

  //thread number for tracing
  int traceTN = 8 * std::thread::hardware_concurrency();

  ifstream myfile(filename);
  if (myfile.is_open())
  {
    myfile.close();
    TraceManager tracer(traceTN, binType, isAAC, thres);
    tracer.traceScene(filename);
  }
  else
  {
    cout << filename << " file not found!" << endl;
  }
  //  system("pause");
  return 0;
}