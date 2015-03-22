#ifndef TASK_H
#define TASK_H

#include <functional>
#include <memory>

#include "Define.h"

class Task
{
private:
    std::function<void ()> func;
    std::shared_ptr<int32> mIdObject  = std::make_shared<int32>();


public:

    Task(uint32 intervall);
    Task();
    void setCallback(std::function<void ()> callback)
    {
        func = callback;
    }
    uint32 Add_Time;
    std::size_t getId() const
    {
        return (std::size_t) mIdObject.get();
    }

    virtual void OnLeave() {}
    virtual void OnAdded() {}
    virtual bool Update(uint32 diff);
    Task(const Task&) = delete;

    uint32 InterVall = 1000;
    uint32 Last_Execute;


protected:


};

#endif // TASK_H
