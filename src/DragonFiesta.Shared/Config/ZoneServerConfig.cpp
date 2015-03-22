#include "ZoneServerConfig.h"
#include "Log/Log.h"
#include "Util/StringExtentsion.h"
#include	<iostream>
#include	<fstream>


ZoneServerConfig::ZoneServerConfig(int32 _ID)
{
    ID = _ID;
}
bool ZoneServerConfig::Read()
{
    try
    {

        ini_file::section_map inimap;

        std::string cnf = MakeString("Config/Zone",GetZoneID(),"Configuration.ini");

        std::ifstream conf_file(cnf);

        if (!conf_file.good())
        {

            sLog->LogMSG(MSG_ERROR,"Can not found %s",cnf.c_str());
            return false;
        }


        std::fstream input;
        input.open(cnf.c_str(), std::fstream::in);


        input >> inimap;
        LogConf  = std::make_shared<LogConfig>();
        NetConf = std::make_shared<NetConfig>();
        InterServerConf = std::make_shared<InterServerConfig>();
        WorldDatabaseConf = std::make_shared<DatabaseConfig>();
        ZoneDatabaseConf = std::make_shared<DatabaseConfig>();

        ZoneMaxCount = std::stoi(inimap["ZoneServer"]["ZoneMaxCount"]);

        if(ZoneMaxCount <= 0)
        {
            sLog->LogMSG(MSG_ERROR,"Invalid ZoneMaxCount must be bigger than 0! Please Check you ZoneServer%s Settings",GetZoneID());
            return false;
        }

        if(!NetConf->Read(inimap)
                || !InterServerConf->ReadConnectSettings(inimap)
                || !WorldDatabaseConf ->Read(inimap,"WorldDatabase")
                || !ZoneDatabaseConf ->Read(inimap,"ZoneDatabase")
                || !LogConf->Read(inimap))
        {
            return false;
        }


        return true;
    }
    catch(...)
    {
        sLog->LogMSG(MSG_ERROR,"Failed to Read ZoneServer%s Config Please Check you ZoneServer%s Settings",GetZoneID(),GetZoneID());
        return false;
    }


}
