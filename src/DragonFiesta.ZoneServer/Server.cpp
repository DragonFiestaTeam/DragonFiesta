#include "Server.h"
#include "Config/ZoneServerConfig.h"
#include "Networking/InterNetwork/InterSocketMgr.h"
#include "Networking/GameNetwork/FiestaSocketMgr.h"
#include "Event/ShutdownEvent.h"
#include "Database/DB.h"
#include "Debugging/CrashHandler.h"
#include "Log/Log.h"
#include "Task/TaskManager.h"
#include "Database/Database.h"
#include "Database/DatabaseManager.hpp"
#include "Database/DatabaseServer.h"
#include "Util/SysInfo.hpp"
#include "ShutdownCodes.h"
#include <algorithm>    // std::find

bool Server::m_stopEvent;
bool Server::Authenticated;
std::vector<std::shared_ptr<ZoneServerConfig>> Server::ZoneConfigs;
std::shared_ptr<ZoneServerConfig> Server::MyServerCfg;

bool Server::Start(boost::asio::io_service& service)
{

    try
    {
//first start crashhandler
        CrashHandler::Start();
        if(!LoadConfigs())
        {
            return false;
        }

        std::shared_ptr<ZoneServerConfig> ServerCfg = ZoneConfigs.front();//Use tmp First ZoneConfig when auth change to my Zone





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


        sLog->LogMSG(MSG_INFO,"ZoneServer Start Success!");


        return true;
    }
    catch(std::exception ex)
    {


        sLog->LogMSG(MSG_CRITICAL_ERROR,"Failed to Start ZoneServer");
        return false;
    }

    return false;
}

bool Server::LoadConfigs()
{
    try
    {
        uint32 ZoneCount = DEFAULT_ZONEMAX;
        for (uint32 i=0; i< ZoneCount; i++)
        {
            std::shared_ptr<ZoneServerConfig> ZoneConfig = std::make_shared<ZoneServerConfig>(i);
            if(ZoneConfig->Read())
            {
                uint32 MaxCount = ZoneConfig->GetZoneMaxCount();

                if(MaxCount != ZoneCount)
                {
                    ZoneCount = MaxCount;
                }
                ZoneConfigs.push_back(ZoneConfig);
            }
            else
            {

                sLog->LogMSG(MSG_ERROR,"Failed To Read Zone%dConfiguration",i);
                return false;
            }
        }

        return true;
    }
    catch(std::exception ex)
    {

        sLog->LogMSG(MSG_CRITICAL_ERROR,"Failed to Start ZoneServer");
        return false;
    }
    return false;
}

bool Server::StartMaintenance(int32 ID,boost::asio::io_service& service)//Here All Stuff After authenticatet
{
    try
    {
        if(!Authenticated)
        {
            return false;
        }

        auto itr = std::find_if( ZoneConfigs.begin(),ZoneConfigs.end(),[ID](std::shared_ptr<ZoneServerConfig>  &cfg)
        {
            if(ID == cfg->GetZoneID())
            {
                return true;
            }
            return false;
        });
        if(itr == std::end(ZoneConfigs))
        {
            sLog->LogMSG(MSG_ERROR,"Faild to get  Zone%dConfiguration",ID);
            return false;
        }



        if(!DB::InitializeWorldDatabase(MyServerCfg->WorldDatabaseConf))
        {

            sLog->LogMSG(MSG_ERROR,"Failed to Initial WorldDatabase! Please Check you Log and you Configuration!");
            return false;
        }


        boost::asio::signal_set signals(service, SIGINT, SIGTERM);
#ifdef _WIN32
        signals.add(SIGBREAK);
#endif
        signals.async_wait(SignalHandler);

        return true;
    }
    catch(std::exception ex)
    {

        sLog->LogMSG(MSG_CRITICAL_ERROR,"Failed to Start ZoneServer");
        return false;
    }

    return false;
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
