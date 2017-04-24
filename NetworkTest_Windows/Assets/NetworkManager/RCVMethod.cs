using System;

public struct RCVMethod
{
    public Action<byte[]> encoding;
    public string name;

    public RCVMethod(Action<byte[]> encoding, string name)
    {
        this.encoding = encoding;
        this.name = name;
    }
}
