#ifndef LOGINSERVER_CONFIG_H
#define LOGINSERVER_CONFIG_H

#include "NetConfig.h"
#include "InterServerConfig.h"
#include "DatabaseConfig.h"
#include "LogConfig.h"



class LoginServerConfig
{
public :
    bool Read();
    std::shared_ptr<NetConfig> NetConf;
    std::shared_ptr<InterServerConfig> InterServerConf;
    std::shared_ptr<DatabaseConfig> DatabaseConf;
    std::shared_ptr<LogConfig> LogConf;

};

#endif
