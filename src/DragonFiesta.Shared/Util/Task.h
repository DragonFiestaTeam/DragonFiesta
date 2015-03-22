#ifndef TASK_H
#define TASK_H

#include <functional>
#include "../Define.h"

class Task
{
public:

   Task(bool Repeat,int Time); // NOTE : Example : Task<True,1000>

   Task(const std::function<void ()>& _Task,bool Repeat,int32 Time,int32 TaskID);
   Task(const std::function<void()>& _Task,int32 RTime,int32 TaskTime,int32 TaskID);
    int32 TaskID;
    bool Repeat = false;
    int32  RTime; //reapeat time
   // int32  Task_Time; //task life time
   // int32  AddTime:
  //  int32  LastExecute;
    virtual void Execute();
    virtual void Leave();
    private :
        const std::function<void ()>& func = NULL;

};



#endif
