#include "ConsoleCommandHandler.h"


ConsoleCommandHandler::ConsoleCommandHandler()
{
}


void ConsoleCommandHandler::RegisterCommand(std::string Name,std::function<bool()> func,std::vector<std::string> Params)
{
}

ConsoleCommandStatus ConsoleCommandHandler::ExecuteCommand(std::string ConsoleInput)
{
}

