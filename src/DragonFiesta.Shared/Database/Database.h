#ifndef DATABASE_H
#define DATABASE_H

#include "../Define.h"
#include <string>

class Database
{
public:

    Database(std::string Name,uint32 MinPoolSize,uint32 MaxPoolSize);
    inline std::string GetName()
    {
        return Name;
    }
    inline uint32 GetMaxPoolSize()
    {
        return MaxPoolSize;
    }
    inline uint32 GetMinPoolSize()
    {
        return MinPoolSize;
    }
private :
    std::string Name;
    uint32 MaxPoolSize;
    uint32 MinPoolSize;
};

#endif
