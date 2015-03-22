#include "DatabaseConfig.h"
#include "Log/Log.h"


bool DatabaseConfig::Read(ini_file::section_map ConfigMap , std::string SectionName)
{

    try
    {


        DatabaseUserName = ConfigMap[SectionName]["DatabaseUser"];
        DatabasePassword = ConfigMap[SectionName]["DatabasePassword"];
        DatabaseName     = ConfigMap[SectionName]["DatabaseName"];
        DatabasePort     = std::stoi(ConfigMap[SectionName]["DatabasePort"]);
        DatabaseHost     = ConfigMap[SectionName]["DatabaseHost"];
        MinPoolSize      = std::stoi(ConfigMap[SectionName]["MinPoolSize"]);
        MaxPoolSize      = std::stoi(ConfigMap[SectionName]["MaxPoolSize"]);


        if(DatabaseUserName  == ""
                || DatabaseName == ""
                || DatabasePort == 0
                || DatabaseHost == ""
                || MinPoolSize  == 0
                || MaxPoolSize  == 0)
        {

            sLog->LogMSG(MSG_ERROR,"Failed to read Database Config Please Check you database Section %x in you Config",SectionName.c_str());
            return false;
        }

        return true;

    }
    catch(...)
    {


        sLog->LogMSG(MSG_ERROR,"Failed to read %x Database Config Please Check you database Settings",SectionName.c_str());
        return false;
    }

}
