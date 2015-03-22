#ifndef SERVER_H
#define SERVER_h

#include "boost/asio.hpp"
#include "Config/WorldServerConfig.h"
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

class Server
{
public :
    static bool Start(boost::asio::io_service& service);
    static bool StartMaintenance(boost::asio::io_service& service);
    static void Shutdown();


private :


    static void SignalHandler(const boost::system::error_code& error, int signalNumber);


protected :
    static bool Authenticated;
    static bool m_stopEvent;
    static void ShutdownInit();
    static std::shared_ptr<WorldServerConfig> ServerCfg;

};

#define DEFAULT_ZONEMAX 3

#endif
