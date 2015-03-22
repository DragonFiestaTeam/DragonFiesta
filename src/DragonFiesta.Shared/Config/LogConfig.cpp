#include "LogConfig.h"

#include "Util/StringExtentsion.h"
#include "Util/Converter.h"


bool LogConfig::Read(ini_file::section_map ini)
{
    try
    {
        LogPath = ini["Log"]["LogPath"];
        LogFileName   = ini["Log"]["LogFileName"];
        MainLogLevel = std::stoi(ini["Log"]["MainLogLevel"]);
        NetworkLogLevel  = std::stoi(ini["Log"]["NetworkLogLevel"]);
        DatabaseLogLevel = std::stoi(ini["Log"]["DatabaseLogLevel"]);
        InterNetworkLogLevel = std::stoi(ini["Log"]["InterNetworkLogLevel"]);

    }
    catch(...)
    {
    sLog->LogMSG(MSG_ERROR,"Failed to Parse Log Settings");
    return false;
    }
    return true;
}


