#ifndef CONSOLE_COMMAND_H
#define CONSOLE_COMMAND_H

#include <vector>

class ConsoleCommand
{
private :
    std::string Command;
    std::vector<std::string> Parameter;
    std::function<bool()>     Func;
public :

    ConsoleCommand(std::string command,std::function<bool()>  func,std::vector<std::string> Params)
    {
        Command = command;
        Func = func;
        Parameter = Params;
    }

};

#endif
