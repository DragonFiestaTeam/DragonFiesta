#include "Task.h"
#include "TaskManager.h"

#include "Timer.h"


Task::Task(uint32 intervall)
{
    if(intervall < 1000)
    {
        throw "Task interval ist out of range";
    }
    this->InterVall = intervall;
}


bool Task::Update(uint32 diff)
{
    if(this->func != nullptr)
    {

        this->func();
    }

    Last_Execute = getMSTime();

    return true;
}
