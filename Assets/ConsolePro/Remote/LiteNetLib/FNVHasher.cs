using System.Collections.Generic;

namespace FlyingWormConsole3.LiteNetLib.Utils
{
    public sealed class FNVHasher : NetSerializerHasher
    {
        private readonly char[] _hashBuffer = new char[1024];
        private readonly Dictionary<string, ulong> _hashCache = new Dictionary<string, ulong>();

        public override ulong GetHash(string type)
        {
            ulong hash;
            if (_hashCache.TryGetValue(type, out hash)) return hash;
            hash = 14695981039346656037UL; //offset
            var len = type.Length;
            type.CopyTo(0, _hashBuffer, 0, len);
            for (var i = 0; i < len; i++)
            {
                hash = hash ^ _hashBuffer[i];
                hash *= 1099511628211UL; //prime
            }

            _hashCache.Add(type, hash);
            return hash;
        }

        public override ulong ReadHash(NetDataReader reader)
        {
            return reader.GetULong();
        }

        public override void WriteHash(ulong hash, NetDataWriter writer)
        {
            writer.Put(hash);
        }
    }
}