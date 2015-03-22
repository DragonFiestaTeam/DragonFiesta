#include "DatabaseClient.hpp"
#include "MySqlException.hpp"
#include "DatabaseExecption.h"
#include "Task/Timer.h"
#include <cassert>
#include <cstdint>
#include <mysql/mysql.h>
#include <string>
#include <sstream>
#include <vector>


using std::string;
using std::vector;


void DatabaseClient::Destroy()
{
    this->Disconnect();
    this->connection_ = nullptr;
}

void DatabaseClient::UpdateLastActivity()
{
    LastActive = getMSTime();
}

void DatabaseClient::Disconnect()
{
    try
    {
        mysql_close(connection_);
        this->State = Closed;
    }
    catch (...)
    {
        this->State = Broken;
    }
}

void DatabaseClient::Connect(
    std::string hostname,
    std::string username,
    std::string password,
    std::string database,
    uint32 port
)
{

    if(hostname == "" || username == "" || database == "")
    {
        throw MySqlException("Invalid Database Connect Informastion");
    }

    connection_ = mysql_init(NULL);


    if (nullptr == connection_)
    {
        throw MySqlException("Unable to connect to MySQL");
    }

    const MYSQL* const success = mysql_real_connect(
                                     connection_,
                                     hostname.c_str(),
                                     username.c_str(),
                                     password.c_str(),
                                     database.c_str(),
                                     port,
                                     NULL,
                                     0);

    if (nullptr == success)
    {
        MySqlException mse(connection_);
        mysql_close(connection_);
        throw mse;
    }
    this->State = Idle;


}

DatabaseClient::~DatabaseClient()
{
//    mysql_close(connection_);
}

DatabaseClient::DatabaseClient(uint32 ID)
{
    this->Handle = ID;
    this->UpdateLastActivity();
    this->State = Closed;

}

my_ulonglong DatabaseClient::runCommand(const char* const command)
{
    if (0 != mysql_real_query(connection_, command, strlen(command)))
    {
        throw MySqlException(connection_);
    }

    // If the user ran a SELECT statement or something else, at least warn them
    const my_ulonglong affectedRows = mysql_affected_rows(connection_);
    if ((my_ulonglong) - 1 == affectedRows)
    {
        // Clean up after the query
        MYSQL_RES* const result = mysql_store_result(connection_);
        mysql_free_result(result);

        throw MySqlException("Tried to run query with runCommand");
    }

    return affectedRows;
}


MySqlPreparedStatement DatabaseClient::prepareStatement(const char* const command) const
{
    return MySqlPreparedStatement(command, connection_);
}
