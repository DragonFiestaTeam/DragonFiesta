#ifndef WRITESTREAM_H
#define WRITESTREAM_H

#include "PacketStream.h"
#include "PacketExecption.h"

class WriteStream : public PacketStream
{
public :
    inline void WriteString(std::string const& str)
    {
        if (size_t len = str.length())
        {
            Write(str.c_str(), len);
        }
    }

    inline void WriteString(std::string const& str,uint32 length)
    {
        Write(str.c_str(),length);
    }

    template <typename T> void Write(T value)//TODO CHECK POD?
    {
        Write((uint8 *)&value, sizeof(value));
    }

    inline void Write(const PacketStream& buffer)
    {
        if (buffer.GetPos())
            Write(buffer.contents(), buffer.GetPos());
    }

    inline void Write(const char *src, size_t cnt)
    {
        return Write((const uint8 *)src, cnt);
    }

    template<class T> void Write(const T *src, size_t cnt)
    {
        return Write((const uint8 *)src, cnt * sizeof(T));
    }

    inline void Write(const uint8 *src, size_t cnt)
    {

        if (!cnt)
            throw PacketStreamSourceException(StreamPos, size(), cnt);

        if (!src)
            throw PacketStreamSourceException(StreamPos, size(), cnt);

        assert(size() < 10000000);


        if (_storage.size() < StreamPos + cnt)
            _storage.resize(StreamPos + cnt);
        std::memcpy(&_storage[StreamPos], src, cnt);
        StreamPos += cnt;
    }
};

#endif
