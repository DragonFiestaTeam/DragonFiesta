#include "TaskManager.h"

#include "SysInfo.hpp"

TaskManager::TaskManager()
{
mFinish = false;

for(int32 i = 0; i < DragonFiesta::SysInfo::GetCPUCount(); i++)
{
mThreads.push_back(std::shared_ptr<std::thread>(new std::thread(&TaskManager::ExecuteTask,this)));
}

}

int TaskManager::AddTask(const std::function<void ()>& _Task,bool Repeat,int Time)
{
    std::unique_lock<std::mutex> l(mMutex);
    mTasks.push_back(new Task(_Task,Repeat,Time,GetFreeTaskID()));
    mCond.notify_one();
}

bool TaskManager::RemoveTask(int32 ID)
{

return true;
}

void TaskManager::ExecuteTask()
{
    bool running = true;
    while (running) {
      std::unique_lock<std::mutex> l(mMutex);
      mCond.wait(l, [this]() {
        return mFinish || mTasks.size() != 0 || ExecuteTask.size() != 0;
      });


      if (!ExecuteTasks.empty()) {
      Task* task = ExecuteTasks.front();
          mTasks.pop_front();
          l.unlock();
//          if(task.AddTime)
           task->Executing();
      }
      else {
         running = !mFinish;
      }
    }

}


void TaskManager::CheckTasks()
{

 for(int32 i=mTasks.begin(); i != mTasks.end(); ++i)
 {
 Task* pTask = mTasks[i];

 if(!pTask->Repeat)
 {
   mTasks.remove(pTask);
 }
 }

}

void TaskManager::JoinAll()
{

    {
      std::unique_lock<std::mutex> l(mMutex);
      mFinish = true;
    }

    mCond.notify_all();
    for (auto t : mThreads) {
      t->join();
    }

}



int32 TaskManager::GetFreeTaskID()
{
/*for(int32 i = 0; i < mTasks.max_size(); i++)
{

if(!mTasks.find(i))
{
return i;
}

}*/

return -1;
}
