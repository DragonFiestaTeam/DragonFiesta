#include "StringExtentsion.h"
#include <string>
#include <vector>
#include <boost/algorithm/string.hpp>

std::vector<std::string> StringUtil::Spliting(std::string src,const char* sep)//what the fuck
{
    std::vector<std::string> strs;
    boost::split( strs, src, boost::is_any_of(sep));
    return strs;
}
