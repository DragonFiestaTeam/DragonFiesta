#ifndef PACKETEXECPTION_H
#define PACKETEXECPTION_H

// Root of ByteBuffer exception hierarchy

#include <exception>

class PacketStreamException : public std::exception
{
public:
    ~PacketStreamException() throw() { }

    char const* what() const throw() override
    {
        return msg_.c_str();
    }

private:
    std::string msg_;
};

class PacketStreamPositionException : public PacketStreamException
{
public:
    PacketStreamPositionException(bool add, size_t pos, size_t size, size_t valueSize);

    ~PacketStreamPositionException() throw() { }
};

class PacketStreamSourceException : public PacketStreamException
{
public:
    PacketStreamSourceException(size_t pos, size_t size, size_t valueSize);

    ~PacketStreamSourceException() throw() { }
};


#endif
