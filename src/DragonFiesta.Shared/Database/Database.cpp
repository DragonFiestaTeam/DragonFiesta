#include "Database.h"

#include "Log/Log.h"


Database::Database(std::string Name,uint32 MinPoolSize,uint32 MaxPoolSize)
{

    if(MinPoolSize == 0 || MaxPoolSize < MinPoolSize)
    {
        sLog->LogMSG(MSG_ERROR,LTAG_DATABASE,"Invalid MinPool or MaxPoolSize please check you database Parameters");
        return;
    }

    this->MaxPoolSize = MaxPoolSize;
    this->MinPoolSize = MinPoolSize;
    this->Name        = Name;
}
