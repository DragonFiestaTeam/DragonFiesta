#ifndef TIMERTASK_H
#define TIMERTASK_H

#include "Task.h"
#include "Timer.h"

class TimerTask : public Task
{
public:
    TimerTask(uint32 intervall,uint32 lifeTime);

    uint32 LifeTime;


  //  virtual void OnLeave() = 0;
    //virtual void OnAdded() = 0;
    //virtual void OnAdded() = 0;
    virtual bool Update(uint32 diff);

    uint32 GetRestLifeTime()
    {
        return getMSTimeDiff(getMSTime(),Add_Time+LifeTime);
    }

    void setCallback(std::function<void ()> callback)
    {
        func = callback;
    }
    std::size_t getId() const
    {
        return (std::size_t) mIdObject.get();
    }
    TimerTask(const Task&) = delete;
protected:

private :
    std::function<void ()> func;
    std::shared_ptr<int32> mIdObject = std::make_shared<int32>();

};

#endif // TIMERTASK_H
