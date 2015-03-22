#ifndef MYSQL_HPP_
#define MYSQL_HPP_

#include <cassert>
#include <cstdint>
#include <cstring>
#include <mysql/mysql.h>

#include <boost/lexical_cast.hpp>
#include <string>
#include <tuple>
#include <typeinfo>
#include <utility>
#include <vector>

#include "InputBinder.hpp"
#include "MySqlException.hpp"
#include "DatabaseClientState.h"
#include "MySqlPreparedStatement.hpp"
#include "OutputBinder.hpp"
#include "../Define.h"
#if __GNUC__ < 4 || (__GNUC__ == 4 && __GNUC_MINOR__ < 6)
#ifndef nullptr
// I know that this isn't a perfect substitution, but I'm lazy
#define nullptr 0
#endif
#endif


class DatabaseClient
{
public:

    void Connect (
        std::string hostname,
        std::string username,
        std::string password,
        std::string database,
        uint32 port = 3306 );
    void Disconnect();

    ~DatabaseClient();
    DatabaseClient(uint32 ID);
    DatabaseClient(const DatabaseClient& rhs) = delete;
    DatabaseClient(DatabaseClient&& rhs) = delete;
    DatabaseClient& operator=(const DatabaseClient& rhs) = delete;
    DatabaseClient& operator=(DatabaseClient&& rhs) = delete;

    /**
     * Normal query. Results are stored in the given vector.
     * @param query The query to run.
     * @param results A vector of tuples to store the results in.
     * @param args Arguments to bind to the query.
     */
    template <typename... InputArgs, typename... OutputArgs>
    void runQuery(
        std::vector<std::tuple<OutputArgs...>>* const results,
        const char* const query,
        // Args needs to be sent by reference, because the values need to be
        // nontemporary (that is, lvalues) so that their memory locations
        // can be bound to DatabaseClient's prepared statement API
        const InputArgs&... args) const;

    /**
     * Command that doesn't return results, like "USE yelp" or
     * "INSERT INTO user VALUES ('Brandon', 28)".
     * @param query The query to run.
     * @param args Arguments to bind to the query.
     * @return The number of affected rows.
     */
    /// @{
    template <typename... Args>
    my_ulonglong runCommand(
        const char* const command,
        // Args needs to be sent by reference, because the values need to be
        // nontemporary (that is, lvalues) so that their memory locations
        // can be bound to DatabaseClient's prepared statement API
        const Args&... args);
    my_ulonglong runCommand(const char* const command);
    /// @}

    /**
     * Prepare a statement for multiple executions with different bound
     * parameters. If you're running a one off query or statement, you
     * should use runQuery or runCommand instead.
     * @param query The query to prepare.
     * @return A prepared statement object.
     */
    MySqlPreparedStatement prepareStatement(const char* statement) const;

    /**
     * Run the command version of a prepared statement.
     */
    /// @{
    template <typename... Args>
    my_ulonglong runCommand(
        const MySqlPreparedStatement& statement,
        const Args&... args);
    my_ulonglong runCommand(const MySqlPreparedStatement& statement);
    /// @}

    /**
     * Run the query version of a prepared statement.
     */
    template <typename... InputArgs, typename... OutputArgs>
    void runQuery(
        std::vector<std::tuple<OutputArgs...>>* results,
        const MySqlPreparedStatement& statement,
        const InputArgs&...) const;
    DatabaseClientState State =  None;
    uint32 LastActive;

    void Destroy();
    uint32 mHandle() { return Handle; }
    MYSQL* GetConnection() { return connection_; }
    void UpdateLastActivity();

private:
    MYSQL* connection_;
    uint32 Handle;

};


template <typename... Args>
my_ulonglong DatabaseClient::runCommand(
    const char* const command,
    const Args&... args
)
{
    MySqlPreparedStatement statement(prepareStatement(command));
    return runCommand(statement, args...);
}


template <typename... Args>
my_ulonglong DatabaseClient::runCommand(
    const MySqlPreparedStatement& statement,
    const Args&... args
)
{
    // Commands (e.g. INSERTs or DELETEs) should always have this set to 0
    if (0 != statement.getFieldCount())
    {
        throw MySqlException("Tried to run query with runCommand");
    }

    if (sizeof...(args) != statement.getParameterCount())
    {
        std::string errorMessage;
        errorMessage += "Incorrect number of parameters; command required ";
        errorMessage += boost::lexical_cast<std::string>(
                            statement.getParameterCount());
        errorMessage += " but ";
        errorMessage += boost::lexical_cast<std::string>(sizeof...(args));
        errorMessage += " parameters were provided.";
        throw MySqlException(errorMessage);
    }

    std::vector<MYSQL_BIND> bindParameters;
    bindParameters.resize(statement.getParameterCount());
    bindInputs<Args...>(&bindParameters, args...);
    if (0 != mysql_stmt_bind_param(
                statement.statementHandle_,
                bindParameters.data())
       )
    {
        throw MySqlException(statement);
    }

    if (0 != mysql_stmt_execute(statement.statementHandle_))
    {
        throw MySqlException(statement);
    }

    // If the user ran a SELECT statement or something else, at least warn them
    const auto affectedRows = mysql_stmt_affected_rows(
                                  statement.statementHandle_);
    if ((static_cast<decltype(affectedRows)>(-1)) == affectedRows)
    {
        throw MySqlException("Tried to run query with runCommand");
    }

    return affectedRows;
}


template <typename... InputArgs, typename... OutputArgs>
void DatabaseClient::runQuery(
    std::vector<std::tuple<OutputArgs...>>* const results,
    const char* const query,
    const InputArgs&... args
) const
{
    assert(nullptr != results);
    assert(nullptr != query);
    MySqlPreparedStatement statement(prepareStatement(query));
    runQuery(results, statement, args...);
}


template <typename... InputArgs, typename... OutputArgs>
void DatabaseClient::runQuery(
    std::vector<std::tuple<OutputArgs...>>* const results,
    const MySqlPreparedStatement& statement,
    const InputArgs&... args
) const
{
    assert(nullptr != results);

    // SELECTs should always return something. Commands (e.g. INSERTs or
    // DELETEs) should always have this set to 0.
    if (0 == statement.getFieldCount())
    {
        throw MySqlException("Tried to run command with runQuery");
    }

    // Bind the input parameters
    // Check that the parameter count is right
    if (sizeof...(InputArgs) != statement.getParameterCount())
    {
        std::string errorMessage;

        errorMessage += "Incorrect number of input parameters; query required ";
        errorMessage += boost::lexical_cast<std::string>(
                            statement.getParameterCount());
        errorMessage += " but ";
        errorMessage += boost::lexical_cast<std::string>(sizeof...(args));
        errorMessage += " parameters were provided.";
        throw MySqlException(errorMessage);
    }

    std::vector<MYSQL_BIND> inputBindParameters;
    inputBindParameters.resize(statement.getParameterCount());
    bindInputs<InputArgs...>(&inputBindParameters, args...);
    if (0 != mysql_stmt_bind_param(
                statement.statementHandle_,
                inputBindParameters.data())
       )
    {
        throw MySqlException(statement);
    }

    setResults<OutputArgs...>(statement, results);
}


#endif  // MYSQL_HPP_
