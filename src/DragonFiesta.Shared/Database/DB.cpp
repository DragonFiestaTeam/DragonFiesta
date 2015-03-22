#include "DB.h"
#include "Config/LoginServerConfig.h"
#include "Config/WorldServerConfig.h"
#include "Config/ZoneServerConfig.h"
#include "Task/TaskManager.h"

std::shared_ptr<DatabaseManager> LoginDBMgr;
std::shared_ptr<DatabaseManager> WorldDBMgr;
std::shared_ptr<DatabaseManager> ZoneDBMgr;

bool DB::InitializeLoginDatabase(std::shared_ptr<DatabaseConfig> DBcfg)
{
    std::shared_ptr<DatabaseServer> mServer = std::make_shared<DatabaseServer>(
                DBcfg->GetHost(),
                DBcfg->GetPort(),
                DBcfg->GetUserName(),
                DBcfg->GetPassword());

    std::shared_ptr<Database> mDatabase = std::make_shared<Database>(
            DBcfg->GetDatabaseName(),
            DBcfg->GetMinPoolSize(),
            DBcfg->GetMaxPoolSize());

    LoginDBMgr = std::make_shared<DatabaseManager>(mDatabase,mServer);

    TaskManager::GetInstance().AddTask(std::shared_ptr<DatabaseManager>(LoginDBMgr));

    std::cout << LoginDBMgr << std::endl;
    return true;
}

bool DB::InitializeWorldDatabase(std::shared_ptr<DatabaseConfig> DBcfg)
{

    std::shared_ptr<DatabaseServer> mServer = std::make_shared<DatabaseServer>(
                DBcfg->GetHost(),
                DBcfg->GetPort(),
                DBcfg->GetUserName(),
                DBcfg->GetPassword());

    std::shared_ptr<Database> mDatabase = std::make_shared<Database>(
            DBcfg->GetDatabaseName(),
            DBcfg->GetMinPoolSize(),
            DBcfg->GetMaxPoolSize());
    std::cout << "Loool" << DBcfg->GetMinPoolSize();
    WorldDBMgr = std::make_shared<DatabaseManager>(mDatabase,mServer);

    TaskManager::GetInstance().AddTask(std::shared_ptr<DatabaseManager>(WorldDBMgr));

    return true;
}

bool DB::InitializeZoneDatabase(std::shared_ptr<DatabaseConfig> DBcfg)
{

    std::shared_ptr<DatabaseServer> mServer = std::make_shared<DatabaseServer>(
                DBcfg->GetHost(),
                DBcfg->GetPort(),
                DBcfg->GetUserName(),
                DBcfg->GetPassword());

    std::shared_ptr<Database> mDatabase = std::make_shared<Database>(
            DBcfg->GetDatabaseName(),
            DBcfg->GetMinPoolSize(),
            DBcfg->GetMaxPoolSize());

    ZoneDBMgr = std::make_shared<DatabaseManager>(mDatabase,mServer);

    TaskManager::GetInstance().AddTask(std::shared_ptr<DatabaseManager>(ZoneDBMgr));

    return true;
}

