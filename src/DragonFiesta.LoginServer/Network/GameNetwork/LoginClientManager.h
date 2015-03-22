#ifndef LOGINCLIENTMANAGER_H
#define LOGINCLIENTMANAGER_H

#include "Networking/GameNetwork/FiestaClientManager.h"
#include "LoginClient.h"

class LoginClientManager : FiestaClientManager<LoginClient>
{
public :
    LoginClientManager();
    ~LoginClientManager();
};




#endif
