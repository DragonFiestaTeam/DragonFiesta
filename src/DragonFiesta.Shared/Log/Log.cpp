#include "Log.h"





Log::Log()
{
    //ctor
}

Log::~Log()
{
    //dtor
}

#include <boost/log/core.hpp>
#include <boost/log/trivial.hpp>
#include <boost/log/utility/setup.hpp>
#include <boost/log/sources/logger.hpp>

std::map<const std::string,int> LogLevelColors
{
    { "DEBUG",           CONSOLE_COLORS::CC_LIGHTMAGENTA },
    { "WARNING",         CONSOLE_COLORS::CC_YELLOW       },
    { "INFO",            CONSOLE_COLORS::CC_GREEN        },
    { "ERROR",           CONSOLE_COLORS::CC_LIGHTRED     },
    { "CRITICAL_ERROR",  CONSOLE_COLORS::CC_RED          },
};


const char* Log::Levelstrings[5] =
{
    "DEBUG",
    "WARNING",
    "INFO",
    "ERROR",
    "CRITICAL_ERROR",
};


const char* Log::Tagstrings[3] =
{
    "DATABASE",
    "NETWORK",
    "INTER_NETWORK" ,
};


bool Log::SetupLogLevel(LogTag tag,int16 Level)
{
    if(Level > 4)
    {
        Log::LogMSG(MSG_ERROR,"Invalid LogLevel Found %d %d",tag,Level);
        return false;
    }
    Log::LogLevels[tag] = Level;

    return true;
}

void Log::LogMSG(severity_level Level,const char* format, ... )
{
    boost::log::sources::severity_logger< severity_level > slg;
    std::stringstream outstream;
    char buf[32768];
    va_list arglist;
    va_start( arglist,format);
    vsnprintf(buf, 32768, format, arglist);
    outstream << buf;
    va_end( arglist );
    BOOST_LOG_SEV(slg, Level) << outstream.str() ;
}

void Log::LogMSG(severity_level Level,LogTag Tag,const char* format, ... )
{

    boost::log::sources::severity_logger< severity_level > slg;
    std::stringstream outstream;
    char buf[32768];
    va_list arglist;
    va_start( arglist,format);
    vsnprintf(buf, 32768, format, arglist);
    outstream << buf;
    va_end( arglist );

    BOOST_LOG_SCOPED_THREAD_TAG("Tag", Tagstrings[Tag]);
    BOOST_LOG_SEV(slg, Level) << outstream.str();
}

void LogConsole(boost::log::record_view const& rec, boost::log::formatting_ostream& strm)
{
    //why must be use stream to extract values -.-
    std::stringstream outstream;
    std::stringstream ColorLevel;
//TODO BOOST CODE STYLE -.-

    outstream << "[" << sLog->LogDateString() << "]";
    if(rec[tag_attr] != "")
    {

        outstream << "[" << rec[tag_attr] << "]";
    }
    outstream << "[" << rec[severity] << "]: " << *rec[boost::log::expressions::smessage];
    //MSG END
    ColorLevel << rec[severity];
    int ConsolColor = LogLevelColors[ColorLevel.str()];
    sLog->SetConsoleColor(ConsolColor);
    std::cout << outstream.str() << std::endl;
}
bool Log_Filter(boost::log::value_ref< severity_level, tag::severity > const& level,boost::log::value_ref< std::string, tag::tag_attr > const& tag)
{

    return (    (level >= sLog->LogLevels[0] && tag == Log::Tagstrings[0])
                ||(level >= sLog->LogLevels[1] && tag == Log::Tagstrings[1])
                ||(level >= sLog->LogLevels[2] && tag == Log::Tagstrings[2])
                ||(level >= sLog->LogLevels[3] && !tag));
}

void Log::SetConsoleColor(int textcolor,int backColor)
{
#ifdef _WIN32
    SetConsoleTextAttribute( GetStdHandle(STD_OUTPUT_HANDLE), textcolor + ( backcolor << 4 ) );
#else
    printf("\033[%02x;%02xm", (textcolor & 0xFF00)>>8, textcolor & 0xFF);
#endif

}


std::string Log::LogDateString()
{
    time_t     now = time(0);
    struct tm  tstruct;
    char       buf[80];
    tstruct = *localtime(&now);

    strftime(buf, sizeof(buf), "%Y-%m-%d.%X", &tstruct);

    return buf;

}

std::ostream& operator<< (std::ostream& strm, severity_level level)
{
    if (static_cast< std::size_t >(level) < sizeof(Log::Levelstrings) / sizeof(*Log::Levelstrings))
        strm << Log::Levelstrings[level];
    else
        strm << static_cast< int >(level);

    return strm;
}

void Log::Initialize()
{

//First Add Console Logging
    boost::shared_ptr< text_sink >   sink = boost::make_shared< text_sink >();

    sink->set_formatter(&LogConsole);
    sink->set_filter(boost::phoenix::bind(&Log_Filter, severity.or_none(), tag_attr.or_none()));
    boost::log::core::get()->add_sink(sink);
}
std::string Log::BuildLogFileName(std::string FileName,std::string LogPath)
{
std::stringstream stm;
stm << LogPath << "/" << FileName << "_" <<  Log::LogDateString() << ".log";
return stm.str();
}

bool Log::Initialize(std::shared_ptr<LogConfig> pConfig)
{

    try
    {
        std::string LogFilename =   Log::BuildLogFileName(pConfig->GetLogFileName(),pConfig->GetLogPath());

        boost::log::formatter fmt = boost::log::expressions::stream << "[" << Log::LogDateString() << "]"
                                    << boost::log::expressions::if_(boost::log::expressions::has_attr(tag_attr))
                                    [
                                        boost::log::expressions::stream << "[" << tag_attr << "]"
                                    ]
                                    << "[" << severity << "]: "
                                    << boost::log::expressions::smessage;

        boost::shared_ptr< text_sink >  sink = boost::make_shared< text_sink >();

       sink->locked_backend()->add_stream(
           boost::make_shared< std::ofstream >(LogFilename));


        sink->set_filter(boost::phoenix::bind(&Log_Filter, severity.or_none(), tag_attr.or_none()));
        boost::log::core::get()->add_sink(sink);


     sink->set_formatter(fmt);

        sLog->SetupLogLevel(LTAG_NETWORK,pConfig->GetNetworkLogLevel());
        sLog->SetupLogLevel(LTAG_DATABASE,pConfig->GetDatabaseLogLevel());
        sLog->SetupLogLevel(LTAG_INTERNETWORK,pConfig->GetInterNetworkLogLevel());
        sLog->SetupLogLevel( LTag_MAINLEVEL,pConfig->GetMainLogLevel());

//sLog->Initialize("testaa.log");
    }
    catch(std::exception ex)
    {

        return false;
    }
    return true;
}
