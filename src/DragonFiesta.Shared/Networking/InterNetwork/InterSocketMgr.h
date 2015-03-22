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
#ifndef __INTERSOCKETMGR_H
#define __INTERSOCKETMGR_H

#include "Networking/SocketMgr.h"
#include "Config/InterServerConfig.h"

class InterSocket;
/// Manages all sockets connected to peers and network threads
class InterSocketMgr : public SocketMgr<InterSocket>
{
    typedef SocketMgr<InterSocket> BaseSocketMgr;
public:
    ~InterSocketMgr();
    static InterSocketMgr& Instance()
    {
        static InterSocketMgr instance;
        return instance;
    }
/// Start network, listen at address:port .
    bool Setup(std::shared_ptr<InterServerConfig> cfg);
    bool StartNetwork(boost::asio::io_service& service) override;
/// Stops all network threads, It will wait for all running threads .
    void StopNetwork() override;
    void OnSocketOpen(tcp::socket&& sock) override;
protected:
    InterSocketMgr();
    NetworkThread<InterSocket>* CreateThreads() const override;
private:
    AsyncAcceptor* _instanceAcceptor;
    int32 _socketSendBufferSize;
    int32 m_SockOutUBuff;
    bool _tcpNoDelay;
    bool Starting;
};
#define sInterSocketMgr InterSocketMgr::Instance()
#endif
/// @}
