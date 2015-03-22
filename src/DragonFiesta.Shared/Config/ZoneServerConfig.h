#ifndef ZONESERVER_CONFIG_H
#define ZONESERVER_CONFIG_H

#include "NetConfig.h"
#include "InterServerConfig.h"
#include "LogConfig.h"
#include "DatabaseConfig.h"


class ZoneServerConfig
{
public :
    ZoneServerConfig(int32 ID);
    bool                                Read();
    std::shared_ptr<LogConfig>          LogConf;
    std::shared_ptr<NetConfig>          NetConf;
    std::shared_ptr<InterServerConfig>  InterServerConf;
    std::shared_ptr<DatabaseConfig>     WorldDatabaseConf;
    std::shared_ptr<DatabaseConfig>     ZoneDatabaseConf;

    uint32                                GetZoneMaxCount()
    {
        return ZoneMaxCount;
    }

    int32 GetZoneID()
    {
        return ID;
    }
private :

    int32                               ZoneMaxCount;
    int32 ID;


};

#endif
