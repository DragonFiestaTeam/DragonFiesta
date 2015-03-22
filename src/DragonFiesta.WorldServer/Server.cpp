#include "Server.h"

bool Server::m_stopEvent;
bool Server::Authenticated;
std::shared_ptr<WorldServerConfig> Server::ServerCfg;


bool Server::Start(boost::asio::io_service& service)
{

    try
    {
        sLog->Initialize(); //Start Console Logging
        CrashHandler::Start();
        ServerCfg = std::make_shared<WorldServerConfig>();

        if(!ServerCfg->Read())
        {
            return false;
        }
        if(!sLog->Initialize(ServerCfg->LogConf))
        {
            return false;
        }

        if(!DB::InitializeZoneDatabase(ServerCfg->ZoneDatabaseConf))
        {
            sLog->LogMSG(MSG_ERROR,"Failed to Initial ZoneDatabase! Please Check you Log and you Configuration!");
            return false;
        }

        if(!sInterSocketMgr.Setup(ServerCfg->InterServerConf) || !sInterSocketMgr.StartNetwork(service,ServerCfg->InterServerConf->GetListenIP(),ServerCfg->InterServerConf->GetListenPort()))
        {
            return false;
        }


    }
    catch(std::exception ex)
    {

        sLog->LogMSG(MSG_CRITICAL_ERROR,"Failed to Start WorldServer");
        return false;
    }

    return true;
}

bool Server::StartMaintenance(boost::asio::io_service& service)
{
    if(!Authenticated)
    {
        return false;
    }

    if(!DB::InitializeWorldDatabase(ServerCfg->WorldDatabaseConf))
    {
        sLog->LogMSG(MSG_ERROR,"Failed to Initial WorldDatabase! Please Check you Log and you Configuration!");
        return false;
    }

    if(!sFiestaSocketMgr.Setup(ServerCfg->NetConf) || !sFiestaSocketMgr.StartNetwork(service,ServerCfg->NetConf->GetListenIP(),ServerCfg->NetConf->GetListenPort()))
    {
        return false;
    }

    return true;
}

void Server::Shutdown()
{
    if(Server::m_stopEvent)
    {
        TaskManager::GetInstance().Stop();

        sLog->LogMSG(MSG_INFO,"Shutdown Success Exiting Console");
        std::exit(SHUTDOWN_EXIT_CODE);
    }
}

void Server::ShutdownInit()
{
    if(!Server::m_stopEvent)
    {
        Server::m_stopEvent = true;
        std::string Reason = "by Console";
        std::shared_ptr<ShutdownEvent> StopEvent = std::make_shared<ShutdownEvent>(Reason,10000,1000); // 10 Sec to Shutdown And display Message All 1 Seconds
        TaskManager::GetInstance().AddTask(StopEvent);
    }

}
void Server::SignalHandler(const boost::system::error_code& error, int /*signalNumber*/)
{

    if (!error)
        Server::ShutdownInit();
}
