#ifndef CONSOLE_COMMAND_HANDLER_H
#define CONSOLE_COMMAND_HANDLER_H

#include "../Util/SecureCollection.hpp"
#include "ConsoleCommand.h"
#include "ConsoleCommandStatus.h"

class ConsoleCommandHandler
{

public :
    ConsoleCommandHandler();
    void RegisterCommand(std::string Name,std::function<bool()> func,std::vector<std::string> Params);
    ConsoleCommandStatus ExecuteCommand(std::string ConsoleInput);
      //  virtual void LoadCommand();
private :

    SecureCollection<std::string,ConsoleCommand> Commands;

};


#endif
