#include "LoginSocket.h"
#include "LoginSocketMgr.h"
#include "Networking/SocketMgr.h"
#include "Networking/networkThread.h"
#include <boost/system/error_code.hpp>
#include "../../Server.h"
#include "LoginClient.h"

static void OnSocketAccept(tcp::socket&& sock)
{
    sLoginSocketMgr.OnSocketOpen(std::forward<tcp::socket>(sock));
}

class LoginSocketThread : public NetworkThread<LoginSocket>
{
public:

    void SocketAdded(std::shared_ptr<LoginSocket> sock) override
    {
        sock->OnOpen();
    }
    void SocketRemoved(std::shared_ptr<LoginSocket> sock) override
    {
        sock->OnClose();
    }
};


LoginSocketMgr::LoginSocketMgr() : BaseSocketMgr(), _socketSendBufferSize(-1), m_SockOutUBuff(65536), _tcpNoDelay(true)
{
}

LoginSocketMgr::~LoginSocketMgr()
{
    delete _instanceAcceptor;
}

bool LoginSocketMgr::Setup(std::shared_ptr<NetConfig> cfg)
{




    return true;

}

bool LoginSocketMgr::StartNetwork(boost::asio::io_service& service)
{

    _tcpNoDelay = ServerCfg->NetConf->GetTcpNoDelay();
    _socketSendBufferSize = ServerCfg->NetConf->GetSendBufferSize();
    m_SockOutUBuff = ServerCfg->NetConf->GetSockOutUBuff();
    sLog->LogMSG(MSG_DEBUG,LTAG_NETWORK, "Max allowed socket connections %d", boost::asio::socket_base::max_connections);

    if ( m_SockOutUBuff <= 0)
    {
        sLog->LogMSG(MSG_ERROR,LTAG_NETWORK,"Network.OutUBuff is wrong in your config file");
        return false;
    }
    _threadCount = ServerCfg->NetConf->GetNetworkThreadCount();

    if (_threadCount <= 0)
    {
       sLog->LogMSG(MSG_ERROR,LTAG_NETWORK, "Network.Threads is wrong in your config file");
        return false;
    }

    BaseSocketMgr::StartNetwork();//NetworkThread :D
    _instanceAcceptor = new AsyncAcceptor(service,ServerCfg->NetConf->GetListenIP(), ServerCfg->NetConf->GetListenPort());
    _instanceAcceptor->AsyncAcceptManaged(&OnSocketAccept);


//    sFiestaServer->OnNetworkStart(bindIp,port);
sLog->LogMSG(MSG_INFO,"Listening LoginServer on %s:%i");
    return true;
}

void LoginSocketMgr::StopNetwork()
{
    BaseSocketMgr::StopNetwork();
//    sFiestaServer->OnNetworkStop();
}

void LoginSocketMgr::OnSocketOpen(tcp::socket&& sock)
{

//// set some options here
    if (_socketSendBufferSize >= 0)
    {
        boost::system::error_code err;
        sock.set_option(boost::asio::socket_base::send_buffer_size(_socketSendBufferSize), err);
        if (err && err != boost::system::errc::not_supported)
        {
            sLog->LogMSG(MSG_ERROR,LTAG_NETWORK,"LoginSocketMgr::OnSocketOpen sock.set_option(boost::asio::socket_base::send_buffer_size) err = %s", err.message().c_str());
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
            sLog->LogMSG(MSG_ERROR,LTAG_NETWORK,"OnSocketOpen sock.set_option(boost::asio::ip::tcp::no_delay) err = %s", err.message().c_str());
            return;
        }
    }
//sock->m_OutBufferSize = static_cast<size_t> (m_SockOutUBuff);
    BaseSocketMgr::OnSocketOpen(std::forward<tcp::socket>(sock));


}

NetworkThread<LoginSocket>* LoginSocketMgr::CreateThreads() const
{
    return new LoginSocketThread[GetNetworkThreadCount()];
}
