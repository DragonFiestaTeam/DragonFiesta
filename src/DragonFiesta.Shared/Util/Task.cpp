#include "Task.h"


/*
Task::Task(const std::function<void ()>& _Task,bool Repeat,int32 Time,int32 TaskID)
{
this->func = _Task;
this->Time = Time;
this->TaskID = TaskID;
}*/

Task::Task(bool Repeat,int32 Time)
{
this->Repeat = Repeat;
//this->Time= Time;
}

void Task::Execute()
{
this->func();
}
