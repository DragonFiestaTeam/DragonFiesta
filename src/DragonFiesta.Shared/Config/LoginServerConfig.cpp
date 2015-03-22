#include "LoginServerConfig.h"
#include "Log/Log.h"
#include	<iostream>
#include	<fstream>



bool LoginServerConfig::Read()
{

    ini_file::section_map inimap;

    const int8* cnf = "Config/LoginConfiguration.ini";

    std::ifstream conf_file(cnf);

    if (!conf_file.good())
    {
        sLog->LogMSG(MSG_ERROR,"Can not found %s",cnf);
        return false;
    }

    std::fstream input;
    input.open(cnf, std::fstream::in);


    input >> inimap;
    NetConf = std::make_shared<NetConfig>(); //nach 11er standart
    InterServerConf = std::make_shared<InterServerConfig>();
    DatabaseConf = std::make_shared<DatabaseConfig>();
    LogConf  = std::make_shared<LogConfig>();
    try
    {

        if(!NetConf->Read(inimap)
                || !InterServerConf->Read(inimap)
                || !DatabaseConf->Read(inimap)
                || !LogConf->Read(inimap))

        {
            return false;
        }


    }
    catch(...)
    {
//Failed to read config
        return false;
    }

    return true;
}
