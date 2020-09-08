using FlyingWormConsole3.LiteNetLib.Utils;

namespace FlyingWormConsole3.LiteNetLib
{
    public struct DisconnectInfo
    {
        public DisconnectReason Reason;
        public int SocketErrorCode;
        public NetDataReader AdditionalData;
    }
}