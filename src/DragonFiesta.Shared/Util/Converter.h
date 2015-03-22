#ifndef CONVERTER_H
#define CONVERTER_H


class BoolString : public std::string
{
public:
    BoolString(const std::string& s)
        :   std::string(s)
    {
        if (s != "0" && s != "1")
        {
            throw std::invalid_argument(s);
        }
    }

    operator bool()
    {
        return *this == "1";
    }
};


#endif
