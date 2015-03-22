#include <iostream>
#include "Server.h"
#include "Util/SysInfo.hpp"
#include <pthread.h>
#include "Task/TaskManager.h"
#include "Database/DB.h"
#include "game/Data/MapInfo.h"
#include "Util/SecureCollection.hpp"
#include "boost/asio.hpp"


boost::asio::io_service _ioService;

int main()
{
    if(Server::Start(_ioService))
    {
        _ioService.run();

    }
    return 1;
}
