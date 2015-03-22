#ifndef DB_H
#define DB_H

#include "DatabaseManager.hpp"
#include "Config/DatabaseConfig.h"

extern std::shared_ptr<DatabaseManager> LoginDBMgr;
extern std::shared_ptr<DatabaseManager> WorldDBMgr;
extern std::shared_ptr<DatabaseManager> ZoneDBMgr;


class DB
{
public :


    static bool InitializeLoginDatabase(std::shared_ptr<DatabaseConfig> DBcfg);
    static bool InitializeWorldDatabase(std::shared_ptr<DatabaseConfig> DBcfg);
    static bool InitializeZoneDatabase(std::shared_ptr<DatabaseConfig> DBcfg);
};



#endif
