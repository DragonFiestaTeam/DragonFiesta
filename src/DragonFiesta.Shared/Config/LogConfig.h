#ifndef LOGCONFIG_H
#define LOGCONFIG_H


#include "../Log/Log.h"
#include "IO/ini_file.hpp"


class LogConfig
{
public :
    bool Read(ini_file::section_map cnfIni);

    int16 GetMainLogLevel()
    {
        return MainLogLevel;
    }
    int16 GetNetworkLogLevel()
    {
        return NetworkLogLevel;
    }
    int16 GetDatabaseLogLevel()
    {
        return DatabaseLogLevel;
    }
    int16 GetInterNetworkLogLevel()
    {
        return InterNetworkLogLevel;
    }
    std::string GetLogPath()
    {
        return LogPath;
    }
    std::string GetLogFileName()
    {
        return LogFileName;
    }

private:


    std::string LogPath;
    std::string LogFileName;


    int16 MainLogLevel = 0;
    int16 NetworkLogLevel  = 0;
    int16 DatabaseLogLevel = 0;
    int16 InterNetworkLogLevel = 0;

};

#endif
