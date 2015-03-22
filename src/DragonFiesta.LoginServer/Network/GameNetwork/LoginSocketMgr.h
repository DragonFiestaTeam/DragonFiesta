/*
* Copyright (C) 2008-2013 TrinityCore <http://www.trinitycore.org/>
* Copyright (C) 2005-2009 MaNGOS <http://getmangos.com/>
*
* This program is free software; you can redistribute it and/or modify it
* under the terms of the GNU General Public License as published by the
* Free Software Foundation; either version 2 of the License, or (at your
* option) any later version.
*
* This program is distributed in the hope that it will be useful, but WITHOUT
* ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
* FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for
* more details.
*
* You should have received a copy of the GNU General Public License along
* with this program. If not, see <http://www.gnu.org/licenses/>.
*/
/** \addtogroup u2w User to World Communication
* @{
* \file WorldSocketMgr.h
* \author Derex <derex101@gmail.com>
*/
#ifndef __LOGINSOCKETMGR_H
#define __LOGINSOCKETMGR_H

#include "Networking/SocketMgr.h"
#include "Config/NetConfig.h"
#include "LoginSocket.h"

class LoginSocket;
/// Manages all sockets connected to peers and network threads
class LoginSocketMgr : public SocketMgr<LoginSocket>
{
    typedef SocketMgr<LoginSocket> BaseSocketMgr;
public:
    ~LoginSocketMgr();
    static LoginSocketMgr& Instance()
    {
        static LoginSocketMgr instance;
        return instance;
    }
/// Start network, listen at address:port .
    bool Setup(std::shared_ptr<NetConfig> cfg);
    bool StartNetwork(boost::asio::io_service& service) override;
/// Stops all network threads, It will wait for all running threads .
    void StopNetwork() override;
    void OnSocketOpen(tcp::socket&& sock) override;
protected:
    LoginSocketMgr();
    NetworkThread<LoginSocket>* CreateThreads() const override;
private:
    AsyncAcceptor* _instanceAcceptor;
    int32 _socketSendBufferSize;
    int32 m_SockOutUBuff;
    bool _tcpNoDelay;
};
#define sLoginSocketMgr LoginSocketMgr::Instance()
#endif
/// @}
