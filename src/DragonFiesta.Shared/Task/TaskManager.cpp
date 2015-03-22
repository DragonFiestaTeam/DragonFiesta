#include "TaskManager.h"


#include <chrono>
#include <thread>

#include "Timer.h"
#include "Log/Log.h"
#include "Util/SysInfo.hpp"

TaskManager::TaskManager()
{
    mFinish = false;

    for(int i = 0; i < DragonFiesta::SysInfo::GetCPUCount(); i++)
    {
        mThreads.push_back(std::shared_ptr<std::thread>(new std::thread(&TaskManager::Executor,this)));
    }

}

void TaskManager::RemoveTask(uint32  TaskID)
{

    auto tTask = mTasks[TaskID];
    mTasks.erase(TaskID);
    tTask->OnLeave();
}



void TaskManager::AddTask(std::shared_ptr<Task> mTask)
{
    std::lock_guard<std::mutex> l(mMutex);

    if(this->mTasks[mTask->getId()] == nullptr)
    {
        mTask->OnAdded();
        uint32 AddTime = getMSTime();
        mTask->Add_Time = AddTime;
        this->mTasks[mTask->getId()] = mTask;
    }


}


void TaskManager::Stop()
{
    mFinish = true;

    for(std::map<uint32,std::shared_ptr<Task>>::iterator itr = mTasks.begin(), itr_end = mTasks.end(); itr != itr_end; ++itr)
    {
        this->RemoveTask(itr->second->getId());
    }
}

void TaskManager::Executor()
{
    uint32 lastCheck = 0;
    uint64 last = 0;
    uint64 TicksPerSecond  = 0;
    uint64 TicksToSleep = 400; //:TODO Dynamic class :)
    uint64 sleep = 150;


    for (ulong i = 0; ; i++)
    {

        if(mFinish) break;


        for(std::map<uint32,std::shared_ptr<Task>>::iterator itr = mTasks.begin(), itr_end = mTasks.end(); itr != itr_end; ++itr)
        {
            {
                std::lock_guard<std::mutex> l(mMutex);

                uint32 diff = getMSTime();

                if(diff - itr->second->Last_Execute >= itr->second->InterVall)
                {
                    if(!itr->second->Update(diff))
                    {
                        this->RemoveTask(itr->second->getId());
                    }

                }
            }
        }


        if (GetMSTimeDiffToNow(lastCheck) >= 1000)
        {
            TicksPerSecond = i - last;
            last = i;
            lastCheck = getMSTime();
            //Log(DEBUG"TicksPerSecond : %d  by Thread : %d  ", TicksPerSecond,pthread_self());
            if (TicksPerSecond <= 100)
            {
                sLog->LogMSG(MSG_WARNING,"Server overload! Only % ticks per second!", TicksPerSecond);
            }
        }


        if (i % TicksToSleep == 0)
        {
            std::this_thread::sleep_for(std::chrono::milliseconds(sleep));
        }

    }
}

TaskManager::~TaskManager()
{
    //dtor
}
