#include "ShutdownEvent.h"
#include "Log/Log.h"
#include "Server.h"

ShutdownEvent::ShutdownEvent(std::string Reason,uint32 Time,uint32 Intervall) : TimerTask(Intervall,Time)
{
    this->ShutdownReason = Reason;
    this->setCallback(std::bind(&ShutdownEvent::Update, this));
}
void ShutdownEvent::OnAdded()
{
    sLog->LogMSG(MSG_INFO,"Shutdown Event Has been Added!");
    NoteReason();
    PrintReason();

}
void ShutdownEvent::Update()
{
sLog->LogMSG(MSG_INFO,"Shutdown in %d Seconds",GetRestLifeTime()/1000);
}

void ShutdownEvent::OnLeave()
{
    Server::Shutdown(); //Leave Rest :D
}

void ShutdownEvent::PrintReason()
{

    sLog->LogMSG(MSG_INFO,"Shutdown By Reason %s",ShutdownReason.c_str());

}

void ShutdownEvent::NoteReason()
{

//Idea Sending Ingame Messages
}

