#include "NetConfig.h"
#include "Log/Log.h"




bool NetConfig::Read(ini_file::section_map ConfigMap)
{

    try
    {
        ListenIP       = ConfigMap["Network"]["ListenIP"];
        ListenPort     = std::stoi(ConfigMap["Network"]["ListenPort"]);
        SendBufferSize  = std::stoi(ConfigMap["Network"]["SendBufferSize"]);
        SockOutUBuff = std::stoi(ConfigMap["Network"]["SockOutUBuff"]);
        NetworkThreads = std::stoi(ConfigMap["Network"]["Threads"]);
        if(ListenIP == "" || ListenPort == 0 || SendBufferSize == 0 || SockOutUBuff == 0 || NetworkThreads == 0)
        {
            sLog->LogMSG(MSG_ERROR,"Failed to read Network Config Please Check in Section Network you Network Settings");
            return false;
        }


    }
    catch(...)
    {
        sLog->LogMSG(MSG_ERROR,"Failed to Read Network Config Please Check you  Section Network  you Network Settings");
        return false;
    }

    return true;
}
