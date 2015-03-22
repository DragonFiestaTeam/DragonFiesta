#include "InterClient.h"

InterClient::InterClient(std::shared_ptr<InterSocket> sock) : ClientBase(sock)
{
}
InterClient::~InterClient()
{
}
