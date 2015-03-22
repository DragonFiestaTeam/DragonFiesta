#ifndef WORLDSERVER_CONFIG_H
#define WORLDSERVER_CONFIG_H

#include "NetConfig.h"
#include "InterServerConfig.h"
#include "DatabaseConfig.h"
#include "LogConfig.h"
#include "Define.h"

class WorldServerConfig
{
public :
    bool Read();

    std::shared_ptr<LogConfig> LogConf;
    std::shared_ptr<NetConfig>                        NetConf;
    std::shared_ptr<InterServerConfig>                InterServerConf;
    std::shared_ptr<DatabaseConfig>                   WorldDatabaseConf;
    std::shared_ptr<DatabaseConfig>                   ZoneDatabaseConf;

    int32                           GetMaxPlayerCount()
    {
        return MaxPlayer;
    }

    int32                           GetMaxZoneCount()
    {
        return MaxZoneCount;
    }

    int8                            GetWorldServerID()
    {
        return WorldServerID;
    }
private :

    int32                           MaxPlayer;
    int32                           MaxZoneCount;
    int8                            WorldServerID;

};

#endif // WORLDSERVER_CONFIG_H
