#ifndef TESTTASK_H
#define TESTTASK_H
#include "Task/Task.h"
#include "Task/TimerTask.h"

class TestTask : public TimerTask
{
public:
    TestTask(uint32 intervall);
    void Update2();
    TestTask();
    void OnLeave() override;
    void OnAdded() override;
    ~TestTask();
protected:
private:
};

#endif // TESTTASK_H
