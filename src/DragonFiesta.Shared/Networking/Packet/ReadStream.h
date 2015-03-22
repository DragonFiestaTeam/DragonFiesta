#ifndef READSTREAM_H
#define READSTREAM_H

#include "PacketStream.h"
#include "PacketExecption.h"

class ReadStream : public PacketStream
{
public :
    template <typename T> T Read()
    {
        T r = Read<T>(StreamPos);
        StreamPos += sizeof(T);
        return r;
    }

    template<typename T>
    inline void Read_skip()
    {
        Read_skip(sizeof(T));
    }

    inline void Read_skip(size_t skip)
    {
        if (StreamPos + skip > size())
            throw PacketStreamPositionException(false, StreamPos, skip, size());
        StreamPos += skip;
    }

    template <typename T> T Read(size_t pos) const
    {
        if (pos + sizeof(T) > size())
            throw PacketStreamPositionException(false, pos, sizeof(T), size());
        T val = *((T const*)&_storage[pos]);
        return val;
    }


    inline std::string ReadString(uint32 length)
    {

        if (StreamPos + length > size())
            throw PacketStreamPositionException(false, StreamPos, length, size());

        if (!length)
            return std::string();

        std::string str((char const*)&_storage[StreamPos], length);
        StreamPos += length;
        return str;
    }


};


#endif
