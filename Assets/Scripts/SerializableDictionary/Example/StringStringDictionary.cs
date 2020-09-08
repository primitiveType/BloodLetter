using System;

[Serializable]
public class StringStringDictionary : SerializableDictionary<string, string>
{
}

#if NET_4_6 || NET_STANDARD_2_0
#endif