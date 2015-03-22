#ifndef MATH_H
#define MATH_H

#include <boost/random.hpp>
#include <boost/random/uniform_real_distribution.hpp>

int32 RandInt(int32 Min,int32 Max)
{
    std::time_t now = std::time(0);
    boost::random::mt19937 gen {static_cast<std::uint32_t>(now)};
    boost::random::uniform_int_distribution<> dist {Min, Max};
    return dist(gen);
}


double RandDouble(double Min,double Max)
{



    std::time_t now = std::time(0);
    boost::random::mt19937 gen {static_cast<std::uint32_t>(now)};
    boost::random::uniform_real_distribution< > dist {Min, Max};
    return dist(gen);
}
#endif
