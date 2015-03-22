#ifndef DATABASE_CONFIG_H
#define DATABASE_CONFIG_H

#include "Define.h"
#include "IO/ini_file.hpp"



class DatabaseConfig
{
public :
    bool                                 Read(ini_file::section_map ConfigMap,std::string SectionName = "Database");

    std::string                          GetUserName()
    {
        return DatabaseUserName;
    }
    std::string                          GetPassword()
    {
        return DatabasePassword;
    }
    std::string                          GetDatabaseName()
    {
        return DatabaseName;
    }
    int32                                GetPort()
    {
        return DatabasePort;
    }
    std::string                          GetHost()
    {
        return DatabaseHost;
    }
    int32                                GetMinPoolSize()
    {
        return MinPoolSize;
    }
    int32                                GetMaxPoolSize()
    {
        return MaxPoolSize;
    }
    int32                                GetWorkThreadCount()
    {
        return WorkThreadCount;
    }
private :

    std::string                          DatabaseUserName;
    std::string                          DatabasePassword;
    std::string                          DatabaseName;
    int32                                DatabasePort;
    std::string                          DatabaseHost;
    int32                                MinPoolSize;
    int32                                MaxPoolSize;
    int32                                WorkThreadCount;
};
#endif
