#ifndef SERVER_H
#define SERVER_h


#include "Config/LoginServerConfig.h"
#include "Handler/LoginHandlerStore.h"
#include "boost/asio.hpp"

class Server
{
public :
    static bool Start(boost::asio::io_service& service);

    static void Test();
    static void Shutdown();


private :


    static void SignalHandler(const boost::system::error_code& error, int signalNumber);


protected :
    static bool m_stopEvent;
    static void ShutdownInit();
};

extern  std::shared_ptr<LoginServerConfig> ServerCfg;
extern std::shared_ptr<LoginHandlerStore>  HandlerStore;



#endif
