namespace FlyingWormConsole3.LiteNetLib
{
    public interface INatPunchListener
    {
        void OnNatIntroductionRequest(NetEndPoint localEndPoint, NetEndPoint remoteEndPoint, string token);
        void OnNatIntroductionSuccess(NetEndPoint targetEndPoint, string token);
    }
}