#ifndef SERVER_H
#define SERVER_h

#include "boost/asio.hpp"
#include "Config/ZoneServerConfig.h"

class Server
{
public :
    static bool Start(boost::asio::io_service& service);
    static bool StartMaintenance(int32 ID,boost::asio::io_service& service);
    static bool LoadConfigs();
    static void Shutdown();


private :


    static void SignalHandler(const boost::system::error_code& error, int signalNumber);


protected :
    static bool Authenticated;
    static bool m_stopEvent;
    static void ShutdownInit();
    static std::vector<std::shared_ptr<ZoneServerConfig>> ZoneConfigs;//Load All Zone Configs For Dynamics :D
    static std::shared_ptr<ZoneServerConfig> MyServerCfg;
};

#define DEFAULT_ZONEMAX 3

#endif
