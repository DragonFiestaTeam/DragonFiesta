#include "InterSocket.h"
#include <boost/asio/ip/tcp.hpp>

using boost::asio::ip::tcp;

uint32 const SizeOfClientHeader[2][2] =
{
    { 2, 0 },
    { 6, 4 }
};

uint32 const SizeOfServerHeader[2] = { sizeof(uint16) + sizeof(uint32), sizeof(uint32) };

InterSocket::InterSocket(tcp::socket&& socket) : Socket(std::move(socket))
{

    _headerBuffer.Resize(SizeOfClientHeader[0][0]);
}

InterSocket::~InterSocket()
{
}


void InterSocket::OnOpen()
{
    sLog->LogMSG(MSG_DEBUG,LTAG_INTERNETWORK,"Incomming InterNetwork Connection %s",GetRemoteAddress().c_str());
}

void InterSocket::OnClose()
{
    sLog->LogMSG(MSG_DEBUG,LTAG_INTERNETWORK,"Close InterNetwork Connection %s ",GetRemoteAddress().c_str());
}


bool InterSocket::ReadHeaderHandler()
{
    ClientPktHeader* header = reinterpret_cast<ClientPktHeader*>(_headerBuffer.GetReadPointer());
    uint32 opcode;
    uint32 _size;


    opcode = header->Normal.Command;
    _size = header->Normal.Size;

    _size -= 4;

    _packetBuffer.Resize(_size);
    return true;
}

bool InterSocket::ReadDataHandler()
{
    return true;
}

void InterSocket::ReadHandler()
{

    if (!IsOpen())
        return;

    MessageBuffer& packet = GetReadBuffer();
    while (packet.GetActiveSize() > 0)
    {
        if (_headerBuffer.GetRemainingSpace() > 0)
        {
            // need to receive the header
            std::size_t readHeaderSize = std::min(packet.GetActiveSize(), _headerBuffer.GetRemainingSpace());
            _headerBuffer.Write(packet.GetReadPointer(), readHeaderSize);
            packet.ReadCompleted(readHeaderSize);

            if (_headerBuffer.GetRemainingSpace() > 0)
            {
                // Couldn't receive the whole header this time.
                assert(packet.GetActiveSize() == 0);
                break;
            }

            // We just received nice new header
            if (!ReadHeaderHandler())
                return;
        }

        // We have full read header, now check the data payload
        if (_packetBuffer.GetRemainingSpace() > 0)
        {
            // need more data in the payload
            std::size_t readDataSize = std::min(packet.GetActiveSize(), _packetBuffer.GetRemainingSpace());
            _packetBuffer.Write(packet.GetReadPointer(), readDataSize);
            packet.ReadCompleted(readDataSize);

            if (_packetBuffer.GetRemainingSpace() > 0)
            {
                // Couldn't receive the whole data this time.
                assert(packet.GetActiveSize() == 0);
                break;
            }
        }

        // just received fresh new payload
        if (!ReadDataHandler())
            return;

        _headerBuffer.Reset();

    }
    AsyncRead();
}

void InterSocket::Start()
{
    AsyncRead();
    MessageBuffer initializer;

    std::unique_lock<std::mutex> dummy(_writeLock, std::defer_lock);
    QueuePacket(std::move(initializer), dummy);

}
