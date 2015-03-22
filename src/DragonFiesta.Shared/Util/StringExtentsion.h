#ifndef STRING_EXTENSION
#define STRING_EXTENSION

#include <sstream>      // std::stringstream, std::stringbuf

#include <string>
#include <vector>


template<typename T>
void parseArgs(std::stringstream& stream, const T& arg)
{
    stream << arg;
}

template<typename T, typename... Rem>
void parseArgs(std::stringstream& stream, const T& arg, const Rem&... rem)
{
    stream << arg;
    parseArgs(stream, rem...);
}

template<typename... Args>
std::string MakeString(const Args&... args)
{
    std::stringstream stream;
    parseArgs(stream, args...);
    return stream.str();
}

class StringUtil
{
public:

    static std::vector<std::string> Spliting(std::string src,const char* sep = " ");

};

#endif // STRING_EXTENSION
