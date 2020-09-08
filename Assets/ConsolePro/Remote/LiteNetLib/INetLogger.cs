using System;

namespace FlyingWormConsole3.LiteNetLib
{
    public interface INetLogger
    {
        void WriteNet(ConsoleColor color, string str, params object[] args);
    }
}