#include "FiestaSocket.h"
#include <boost/asio/ip/tcp.hpp>
#include "Log/Log.h"
#include "FiestaClient.h"
using boost::asio::ip::tcp;


uint32 const SizeOfClientHeader[2][2] =
{
    { 2, 0 },
    { 6, 4 }
};

uint32 const SizeOfServerHeader[2] = { sizeof(uint16) + sizeof(uint32), sizeof(uint32) };

FiestaSocket::FiestaSocket(tcp::socket&& socket) : Socket(std::move(socket))
{

    _headerBuffer.Resize(SizeOfClientHeader[0][0]);
}

FiestaSocket::~FiestaSocket()
{
}

void FiestaSocket::OnOpen()
{
    sLog->LogMSG(MSG_DEBUG,LTAG_NETWORK,"Incomming GameNetwork Connection %s",GetRemoteAddress().c_str());
    SendHandShake();
    OnConnect();
}

void FiestaSocket::OnClose()
{
    sLog->LogMSG(MSG_DEBUG,LTAG_NETWORK,"Close GameNetwork Connection %s ",GetRemoteAddress().c_str());
    OnDisconnect();
}

bool FiestaSocket::ReadHeaderHandler()
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

bool FiestaSocket::ReadDataHandler()
{

std::cout << _packetBuffer.GetActiveSize();
    return true;
}

void FiestaSocket::ReadHandler()
{

    if (!IsOpen())
        return;

    if(HandShaked)
    {
        MessageBuffer& packet = GetReadBuffer();
        sLog->LogMSG(MSG_DEBUG,LTAG_NETWORK,"packetsize %i %i",packet.GetActiveSize(),_headerBuffer.GetRemainingSpace());


  /*          if (_headerBuffer.GetRemainingSpace() > 0)
            {
                // need to receive the header
                std::size_t readHeaderSize = std::min(packet.GetActiveSize(), _headerBuffer.GetRemainingSpace());
                _headerBuffer.Write(packet.GetReadPointer(), readHeaderSize);
                packet.ReadCompleted(readHeaderSize);

                if (_headerBuffer.GetRemainingSpace() > 0)
                {
                    // Couldn't receive the whole header this time.
                    assert(packet.GetActiveSize() == 0);

                }

                // We just received nice new header
                if (!ReadHeaderHandler())
                    return;
            }
                        _headerBuffer.Reset();
*/
  /*      while (packet.GetActiveSize() > 0)
        {
            if (_headerBuffer.GetRemainingSpace() > 0)d
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

        }*/
    }
    else
    {
        SendHandShake();
    }

    AsyncRead();
}

//WriteStuff

void FiestaSocket::SendHandShake()
{
std::shared_ptr<FiestaServerPacket> pPacket = std::make_shared<FiestaServerPacket>(1,1);
pPacket->Write<uint32>(1);
SendPacket(pPacket);
    HandShaked = true;
//Later
}

void FiestaSocket::SendPacket(std::shared_ptr<FiestaServerPacket> pPacket)
{
/*    uint32 packetSize = pPacket.size();

    std::unique_lock<std::mutex> guard(_writeLock);

#ifndef DF_SOCKET_USE_IOCP
    if (_writeQueue.empty() && _writeBuffer.GetRemainingSpace() >= packetSize)
        WritePacketToBuffer(pPacket, _writeBuffer);
    else
#endif
    {
        MessageBuffer buffer(packetSize);
        WritePacketToBuffer(pPacket, buffer);
        QueuePacket(std::move(buffer), guard);
    }*/
}

void FiestaSocket::WritePacketToBuffer(std::shared_ptr<FiestaServerPacket> pPacket,MessageBuffer& pBuffer)
{
/*
 uint32 opcode = 0x100;//make later read real opcode :D
    uint32 packetSize = pPacket.size();
//enrcrypt later
 if (!pPacket.empty())
        pBuffer.Write(pPacket.contents(), pPacket.size());

    //tccode like

    /*    ServerPktHeader header;
    uint32 sizeOfHeader = SizeOfServerHeader[_authCrypt.IsInitialized()];
    uint32 opcode = packet.GetOpcode();
    uint32 packetSize = packet.size();

    // Reserve space for buffer
    uint8* headerPos = buffer.GetWritePointer();
    buffer.WriteCompleted(sizeOfHeader);

    if (packetSize > 0x400)
    {
        CompressedWorldPacket cmp;
        cmp.UncompressedSize = packetSize + 4;
        cmp.UncompressedAdler = adler32(adler32(0x9827D8F1, (Bytef*)&opcode, 4), packet.contents(), packetSize);

        // Reserve space for compression info - uncompressed size and checksums
        uint8* compressionInfo = buffer.GetWritePointer();
        buffer.WriteCompleted(sizeof(CompressedWorldPacket));

        uint32 compressedSize = CompressPacket(buffer.GetWritePointer(), packet);

        cmp.CompressedAdler = adler32(0x9827D8F1, buffer.GetWritePointer(), compressedSize);

        memcpy(compressionInfo, &cmp, sizeof(CompressedWorldPacket));
        buffer.WriteCompleted(compressedSize);
        packetSize = compressedSize + sizeof(CompressedWorldPacket);

        opcode = SMSG_COMPRESSED_PACKET;
    }
    else if (!packet.empty())
        buffer.Write(packet.contents(), packet.size());

    if (_authCrypt.IsInitialized())
    {
        header.Normal.Size = packetSize;
        header.Normal.Command = opcode;
        _authCrypt.EncryptSend((uint8*)&header, sizeOfHeader);
    }
    else
    {
        header.Setup.Size = packetSize + 4;
        header.Setup.Command = opcode;
    }

    memcpy(headerPos, &header, sizeOfHeader);*/
}

void FiestaSocket::Start()
{
    AsyncRead();
    MessageBuffer initializer;

    std::unique_lock<std::mutex> dummy(_writeLock, std::defer_lock);
    QueuePacket(std::move(initializer), dummy);

}
