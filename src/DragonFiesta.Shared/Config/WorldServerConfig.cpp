#include "WorldServerConfig.h"
#include "DatabaseConfig.h"
#include "Log/Log.h"

#include	<iostream>
#include	<fstream>


bool WorldServerConfig::Read()
{

    try
    {

        ini_file::section_map inimap;

        const int8* cnf = "Config/WorldConfiguration.ini";

       std::ifstream conf_file(cnf);

        if (!conf_file.good())
        {
            sLog->LogMSG(MSG_ERROR,"Can not found %s",cnf);
            return false;
        }
        std::fstream input;
        input.open(cnf, std::fstream::in);


        input >> inimap;
        LogConf  = std::make_shared<LogConfig>();
        NetConf = std::make_shared<NetConfig>();
        InterServerConf = std::make_shared<InterServerConfig>();
        WorldDatabaseConf = std::make_shared<DatabaseConfig>();
        ZoneDatabaseConf = std::make_shared<DatabaseConfig>();

        WorldServerID = std::stoi(inimap["WorldServer"]["WorldID"]);
        MaxPlayer = std::stoi(inimap["WorldServer"]["MaxPlayer"]);
        MaxZoneCount = std::stoi(inimap["WorldServer"]["MaxZoneCount"]);

        if(!NetConf->Read(inimap)
                || !InterServerConf->Read(inimap)
                || !InterServerConf->ReadConnectSettings(inimap)
                || !WorldDatabaseConf ->Read(inimap,"WorldDatabase")
                || !ZoneDatabaseConf ->Read(inimap,"ZoneDatabase")
                || !LogConf->Read(inimap))
        {
//Handled by Subclasses
            return false;
        }

        if(GetWorldServerID() == -1)
        {
            sLog->LogMSG(MSG_ERROR,"Invalid WorldServer ID Please change you WorldServerID");
            return false;
        }
        if(GetMaxZoneCount() <= 0)
        {
            sLog->LogMSG(MSG_ERROR,"Invialid MaxZoneCount! MaxCount must be bigger than 0");
            return false;
        }


    }
    catch(...)
    {
        sLog->LogMSG(MSG_ERROR,"Failed Load WorldServer Config Please Check you WorldServer Settings!");
        return false;
    }

    return true;
}


