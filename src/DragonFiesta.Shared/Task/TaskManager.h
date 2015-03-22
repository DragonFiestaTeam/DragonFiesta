#ifndef TASKMANAGER_H
#define TASKMANAGER_H

#include "Task.h"
#include "TimerTask.h"

#include <map>
#include <iostream>
#include <deque>
#include <memory>
#include <functional>
#include <chrono>
#include <thread>
#include <mutex>
#include <condition_variable>
#include <list>
#include "Util/Singleton.h"

//NOTE std::chrono::high_resolution_clock::now als id? oder getMSTime()?
class TaskManager : public Singleton<TaskManager>
{
public:
    TaskManager();
    ~TaskManager();
    void    AddTask(std::shared_ptr<Task> mTask);

    void    AddTask(std::function<void()> _Task,uint32 intervall);
    void    AddTask(std::function<void()> _Task,uint32 intervall,uint32 lifeTime);
    void    RemoveTask(uint32  TaskID);
    void    Stop();
    int32 GetTaskCount()
    {
        return mTasks.size();
        mFinish = true;
    }
protected:
private:
    std::map<uint32,std::shared_ptr<Task>> mTasks;
    std::mutex mMutex;

    std::list<std::shared_ptr<std::thread>> mThreads;
    bool mFinish;
    virtual void Executor();

};

#endif // TASKMANAGER_H
