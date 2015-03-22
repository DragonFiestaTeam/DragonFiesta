#include "TimerTask.h"
#include "Timer.h"
#include "TaskManager.h"

TimerTask::TimerTask(uint32 intervall,uint32 lifeTime) : Task(intervall)
{
    this->LifeTime = lifeTime;
    this->InterVall = intervall;

}

bool TimerTask::Update(uint32 diff)
{
    if(getMSTimeDiff(this->Add_Time,diff) > LifeTime)
    {
        return false;
    }
    if(getMSTimeDiff(this->Last_Execute,diff) >= InterVall)
    {
        if(this->func != nullptr)
        {
            this->func();

        }
        Last_Execute = getMSTime();
    }


    return true;
}


