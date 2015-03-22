#ifndef LOG_H
#define LOG_H




#include "../Define.h"
#include "Config/LogConfig.h"

#include <string>
#include <ostream>
#include <fstream>
#include <iostream>
#include <memory>
#include <boost/log/expressions.hpp>
#include <boost/log/sources/severity_logger.hpp>
#include <boost/log/sources/record_ostream.hpp>
#include <boost/log/attributes/scoped_attribute.hpp>
#include <boost/log/sinks/text_ostream_backend.hpp>
#include <boost/log/sinks/sync_frontend.hpp>
#include <boost/log/expressions/keyword.hpp>
#include <boost/date_time/posix_time/posix_time.hpp>
#include <boost/phoenix/bind.hpp>

enum severity_level
{
    MSG_DEBUG ,
    MSG_WARNING,
    MSG_INFO,
    MSG_ERROR,
    MSG_CRITICAL_ERROR
};

enum LogTag
{
    LTAG_DATABASE,
    LTAG_NETWORK ,
    LTAG_INTERNETWORK,
    LTag_MAINLEVEL
};




typedef boost::log::sinks::synchronous_sink< boost::log::sinks::text_ostream_backend > text_sink;



typedef enum
{
    CC_DARKGRAY = 0x0130,
    CC_LIGHTRED = 0x0131,
    CC_LIGHTGREEN = 0x0132,
    CC_YELLOW = 0x0133,
    CC_LIGHTBLUE = 0x0134,
    CC_LIGHTMAGENTA = 0x0135,
    CC_LIGHTCYAN = 0x0136,
    CC_WHITE = 0x0137,
    CC_BLACK = 0x2230,
    CC_RED = 0x2231,
    CC_GREEN = 0x2232,
    CC_BROWN = 0x2233,
    CC_BLUE = 0x2234,
    CC_MAGENTA = 0x2235,
    CC_CYAN = 0x2236,
    CC_LIGHTGRAY = 0x2237
} CONSOLE_COLORS;

BOOST_LOG_ATTRIBUTE_KEYWORD(severity, "Severity", severity_level)
BOOST_LOG_ATTRIBUTE_KEYWORD(tag_attr, "Tag", std::string)
BOOST_LOG_ATTRIBUTE_KEYWORD(timestamp, "TimeStamp", boost::posix_time::ptime)

class LogConfig;

class Log
{
public:
    Log();
    ~Log();



    void Initialize(); //first initial console :D
    bool Initialize(std::shared_ptr<LogConfig>  pConfig);
    bool SetupLogLevel(LogTag tag,int16 Level);
    void LogMSG(severity_level Level,const char* format, ... );
    void LogMSG(severity_level Level,LogTag Tag,const char* format, ... );
    void SetConsoleColor( int textcolor, int backcolor = 0 );
    std::string LogDateString();

    static Log* instance()
    {
        static Log instance;
        return &instance;
    }
    int LogLevels[4] =
    {
        0,//DATABASE
        0,//NETWORK
        0,//INTERNETWORK
        0,//MainLevel
    };
    static const char* Levelstrings[5];
    static const char* Tagstrings[3];

    static std::map<const std::string,int> LogLevelColors;


protected:
private:


std::string BuildLogFileName(std::string FileName,std::string LogPath);




};


#define sLog Log::instance()



#endif // LOG_H
