using System;

public struct ReceiveData
{
    public byte[] data;

    public ReceiveData(byte[] data, int dataLength)
    {
        Array.Clear(data, dataLength + 1, data.Length - dataLength);
        this.data = data;
    }
}