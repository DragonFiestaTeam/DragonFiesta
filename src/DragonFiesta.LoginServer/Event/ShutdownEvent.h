#ifndef SHUTDOWNEVENT_H
#define SHUTDOWNEVENT_H

#include "Task/TimerTask.h"
#include "Task/Task.h"

class ShutdownEvent : public TimerTask
{
public :
    ShutdownEvent() = delete;
    ShutdownEvent(std::string Reason,uint32 Time,uint Intervall);



    void Update();
    void OnLeave() override;
    void OnAdded() override;
private :
    std::string ShutdownReason;
    void PrintReason();
    void NoteReason();
};

#endif
