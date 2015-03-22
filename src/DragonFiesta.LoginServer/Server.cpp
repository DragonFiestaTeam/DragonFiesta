#include "Server.h"
#include "Util/SecureVectorCollection.hpp"
#include "Util/SecureCollection.hpp"
#include "Networking/InterNetwork/InterSocketMgr.h"
#include "Event/ShutdownEvent.h"
#include "Database/DB.h"
#include "Debugging/CrashHandler.h"
#include "Log/Log.h"
#include "Task/TaskManager.h"
#include "TestTask.h"
#include "TestTask2.h"
#include "Database/Database.h"

#include "Database/DatabaseManager.hpp"
#include "Database/DatabaseServer.h"
#include "Util/SysInfo.hpp"
#include "ShutdownCodes.h"
#include <vector>
#include <map>
#include <mutex>
#include <list>
#include <tuple>
#include <thread>
#include <cstdlib>
#include <mutex>
#include "LoginConsole.h"
#include "Network/GameNetwork/LoginSocketMgr.h"
#include "Handler/LoginHandlerStore.h"

std::shared_ptr<LoginServerConfig> ServerCfg;
std::shared_ptr<LoginHandlerStore>  HandlerStore;

bool Server::m_stopEvent;

void Server::Test()
{



//  std::vector<int32> myAll = TestList.FindAll(f);
    /*for(auto e : myAll)
    {
    std::cout << e << std::endl;
    }*/
    //  std::cout << "myZahl" << m;
    /*
        std::vector<std::tuple<std::string, int>> users;
        for(int i = 0; i < 10; i++)
        {


            LoginDBMgr->RunQuery(&users, "SELECT name, age FROM user");
            int age = 29;
            std::string username = "brandon'; DROP TABLE user; -- ";
            LoginDBMgr->runCommand("UPDATE user SET age = ? WHERE name = ?", age, username);

    //LoginDBMgr->runCommand("CREATE DATABASE test_mysql_cpp");
            users.clear();*/

}

bool Server::Start(boost::asio::io_service& service)
{
    try
    {
    sLog->Initialize(); //Start Console Logging
//first start crashhandler
        CrashHandler::Start();

//Load Main Config :)
        ServerCfg = std::make_shared<LoginServerConfig>();
        std::shared_ptr<TestTask> m = std::make_shared<TestTask>();
        TaskManager::GetInstance().AddTask(m);
        boost::asio::signal_set signals(service, SIGINT, SIGTERM);
#ifdef _WIN32
        signals.add(SIGBREAK);
#endif
        signals.async_wait(SignalHandler);


        Test();

        if(!ServerCfg->Read())
        {
            return false;
        }
        if(!sLog->Initialize(ServerCfg->LogConf))
        {
        return false;
        }

        if(!DB::InitializeLoginDatabase(ServerCfg->DatabaseConf))
        {
            sLog->LogMSG(MSG_ERROR,"Failed to Initial LoginDatabase! Please Check you Log and you Configuration!");
            return false;
        }

        HandlerStore = std::make_shared<LoginHandlerStore>();
        HandlerStore->InitialHandlers();

        std::cout << ServerCfg->InterServerConf->GetListenPort() << std::endl;
        /*if(!sInterSocketMgr.Setup(ServerCfg->InterServerConf) || !sInterSocketMgr.StartNetwork(service,ServerCfg->InterServerConf->GetListenIP(),ServerCfg->InterServerConf->GetListenPort()))
        {
            return false;
        }*/
        if(!sLoginSocketMgr.StartNetwork(service))
        {
            return false;
        }

       LoginConsole::GetInstance().Start();
        sLog->LogMSG(MSG_INFO,"LoginServer Start Success!");

SecureWriteCollection<int32,std::string> SecureC;

        SecureC.TryAdd(1,"SayHallo");
        SecureC.TryAdd(2,"SayHallo");
        SecureC.TryAdd(4,"SayHallo");
        SecureC.TryAdd(3,"SayHallo");
SecureC.print();
        //std::cout << SecureC[1] << std::endl;
        SecureVectorCollection<int32> TestList;
        TestList.Add(1);
        TestList.Add(22);
        TestList.Add(221);
        TestList.Add(90);

        return true;
    }
    catch(std::exception ex)
    {

        sLog->LogMSG(MSG_CRITICAL_ERROR,"Failed to Start LoginServer");
        return false;
    }

    return false;
}

void Server::SignalHandler(const boost::system::error_code& error, int /*signalNumber*/)
{

    if (!error)
        Server::ShutdownInit();
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


