namespace FlyingWormConsole3.LiteNetLib
{
    public class EventBasedNatPunchListener : INatPunchListener
    {
        public delegate void OnNatIntroductionRequest(NetEndPoint localEndPoint, NetEndPoint remoteEndPoint,
            string token);

        public delegate void OnNatIntroductionSuccess(NetEndPoint targetEndPoint, string token);

        void INatPunchListener.OnNatIntroductionRequest(NetEndPoint localEndPoint, NetEndPoint remoteEndPoint,
            string token)
        {
            if (NatIntroductionRequest != null)
                NatIntroductionRequest(localEndPoint, remoteEndPoint, token);
        }

        void INatPunchListener.OnNatIntroductionSuccess(NetEndPoint targetEndPoint, string token)
        {
            if (NatIntroductionSuccess != null)
                NatIntroductionSuccess(targetEndPoint, token);
        }

        public event OnNatIntroductionRequest NatIntroductionRequest;
        public event OnNatIntroductionSuccess NatIntroductionSuccess;
    }
}