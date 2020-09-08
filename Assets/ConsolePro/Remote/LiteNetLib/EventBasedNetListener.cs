using FlyingWormConsole3.LiteNetLib.Utils;

namespace FlyingWormConsole3.LiteNetLib
{
    public class EventBasedNetListener : INetEventListener
    {
        public delegate void OnNetworkError(NetEndPoint endPoint, int socketErrorCode);

        public delegate void OnNetworkLatencyUpdate(NetPeer peer, int latency);

        public delegate void OnNetworkReceive(NetPeer peer, NetDataReader reader);

        public delegate void OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader,
            UnconnectedMessageType messageType);

        public delegate void OnPeerConnected(NetPeer peer);

        public delegate void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo);

        void INetEventListener.OnPeerConnected(NetPeer peer)
        {
            if (PeerConnectedEvent != null)
                PeerConnectedEvent(peer);
        }

        void INetEventListener.OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            if (PeerDisconnectedEvent != null)
                PeerDisconnectedEvent(peer, disconnectInfo);
        }

        void INetEventListener.OnNetworkError(NetEndPoint endPoint, int socketErrorCode)
        {
            if (NetworkErrorEvent != null)
                NetworkErrorEvent(endPoint, socketErrorCode);
        }

        void INetEventListener.OnNetworkReceive(NetPeer peer, NetDataReader reader)
        {
            if (NetworkReceiveEvent != null)
                NetworkReceiveEvent(peer, reader);
        }

        void INetEventListener.OnNetworkReceiveUnconnected(NetEndPoint remoteEndPoint, NetDataReader reader,
            UnconnectedMessageType messageType)
        {
            if (NetworkReceiveUnconnectedEvent != null)
                NetworkReceiveUnconnectedEvent(remoteEndPoint, reader, messageType);
        }

        void INetEventListener.OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            if (NetworkLatencyUpdateEvent != null)
                NetworkLatencyUpdateEvent(peer, latency);
        }

        public event OnPeerConnected PeerConnectedEvent;
        public event OnPeerDisconnected PeerDisconnectedEvent;
        public event OnNetworkError NetworkErrorEvent;
        public event OnNetworkReceive NetworkReceiveEvent;
        public event OnNetworkReceiveUnconnected NetworkReceiveUnconnectedEvent;
        public event OnNetworkLatencyUpdate NetworkLatencyUpdateEvent;
    }
}