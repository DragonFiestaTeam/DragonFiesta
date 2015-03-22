#ifndef TASK_MANAGER_H
#define TASK_MANAGER_H

#include <iostream>
#include <deque>
#include <memory>
#include <functional>
#include <chrono>
#include <thread>
#include <mutex>
#include <condition_variable>
#include <list>

#include "../Define.h"
#include "Task.h"

class TaskManager
{



      explicit TaskManager();

      int32 AddTask(const std::function<void ()>& _Task,bool Repeat,int Time);
      bool  RemoveTask(int32 TaskID);
      int32 GetFreeTaskID();
      void JoinAll();


private:
      std::deque<Task*> ExecuteTasks;
      std::map<int,Task*> mTasks;
      std::mutex mMutex;
      std::condition_variable mCond;
      std::list<std::shared_ptr<std::thread>> mThreads;
      bool mFinish;



     void CheckTasks();
      void ExecuteTask();

};


#endif
