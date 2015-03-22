#include "InterSocket.h"
#include "InterSocketMgr.h"
#include "../Networking/SocketMgr.h"
#include "../Networking/networkThread.h"
#include <boost/system/error_code.hpp>


static void OnSocketAccept(tcp::socket&& sock)
{
    sInterSocketMgr.OnSocketOpen(std::forward<tcp::socket>(sock));
}


class InterSocketThread : public NetworkThread<InterSocket>
{
public:

    void SocketAdded(std::shared_ptr<InterSocket> sock) override
    {

        sock->OnOpen();
    }
    void SocketRemoved(std::shared_ptr<InterSocket> sock) override
    {
        sock->OnClose();
    }
};


InterSocketMgr::InterSocketMgr() : BaseSocketMgr(), _socketSendBufferSize(-1), m_SockOutUBuff(65536), _tcpNoDelay(true)
{
}

InterSocketMgr::~InterSocketMgr()
{
    delete _instanceAcceptor;
}

bool InterSocketMgr::Setup(std::shared_ptr<InterServerConfig> cfg)
{


    _tcpNoDelay = cfg->GetTcpNoDelay();
    _socketSendBufferSize = cfg->GetSendBufferSize();
    m_SockOutUBuff = cfg->GetSockOutUBuff();
    sLog->LogMSG(MSG_DEBUG,LTAG_INTERNETWORK,"Max allowed socket connections %d", boost::asio::socket_base::max_connections);

    if ( m_SockOutUBuff <= 0)
    {
        sLog->LogMSG(MSG_ERROR,LTAG_INTERNETWORK,"Network.OutUBuff is wrong in your config file");
        return false;
    }
    _threadCount = cfg->GetNetworkThreadCount();

    if (_threadCount <= 0)
    {
        sLog->LogMSG(MSG_ERROR,LTAG_INTERNETWORK, "Network.Threads is wrong in your config file");
        return false;
    }
    Starting = true;
    return true;

}

bool InterSocketMgr::StartNetwork(boost::asio::io_service& service)
{

    if(!Starting)
    {
        sLog->LogMSG(MSG_ERROR,LTAG_INTERNETWORK,"Please Setup First Network before Starting");
        return false;
    }
    BaseSocketMgr::StartNetwork();
//    _instanceAcceptor = new AsyncAcceptor(service, bindIp, port);
   // _instanceAcceptor->AsyncAcceptManaged(&OnSocketAccept);
//    sInterServer->OnNetworkStart(bindIp,port);
    return true;
}

void InterSocketMgr::StopNetwork()
{
    BaseSocketMgr::StopNetwork();
//    sInterServer->OnNetworkStop();
}

void InterSocketMgr::OnSocketOpen(tcp::socket&& sock)
{

// set some options here
    if (_socketSendBufferSize >= 0)
    {
        boost::system::error_code err;
        sock.set_option(boost::asio::socket_base::send_buffer_size(_socketSendBufferSize), err);
        if (err && err != boost::system::errc::not_supported)
        {
            sLog->LogMSG(MSG_ERROR,LTAG_INTERNETWORK,"InterocketMgr::OnSocketOpen sock.set_option(boost::asio::socket_base::send_buffer_size) err = %s", err.message().c_str());
            return;
        }
    }
// Set TCP_NODELAY.
    if (_tcpNoDelay)
    {
        boost::system::error_code err;
        sock.set_option(boost::asio::ip::tcp::no_delay(true), err);
        if (err)
        {
            sLog->LogMSG(MSG_ERROR,LTAG_INTERNETWORK,"OnSocketOpen sock.set_option(boost::asio::ip::tcp::no_delay) err = %s", err.message().c_str());
            return;
        }
    }
//sock->m_OutBufferSize = static_cast<size_t> (m_SockOutUBuff);
    BaseSocketMgr::OnSocketOpen(std::forward<tcp::socket>(sock));


}

NetworkThread<InterSocket>* InterSocketMgr::CreateThreads() const
{
    return new InterSocketThread[GetNetworkThreadCount()];
}
