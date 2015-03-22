#ifndef LOGINCONSOLE_H
#define LOGINCONSOLE_H

#include <memory>
#include <thread>
#include "Console/ConsoleCommandHandler.h"
#include "Util/Singleton.h"

class LoginConsole : public Singleton<LoginConsole>
{
public :
    void ReadInput();
    void Stop();
    void Start();
private :
    std::shared_ptr<ConsoleCommandHandler> Handler;
    std::shared_ptr<std::thread> MyThread;
    bool Running;


};

#endif
