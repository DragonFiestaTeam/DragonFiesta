#ifndef INTERCLIENTMANAGERBASE_H
#define INTERCLIENTMANAGERBASE_H

#include "InterClient.h"
#include "../ClientManager.h"
#include "../../Util/Singleton.h"

class InterClientManager : public ClientManager<InterClient>
{
public :
    InterClientManager();
    ~InterClientManager();

    static InterClientManager* instance()
    {
        static InterClientManager instance;
        return &instance;
    }

};

#define sInterClientManager InterClientManager::instance()

#endif
