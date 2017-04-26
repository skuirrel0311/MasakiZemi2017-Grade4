using System;
using UnityEngine;

public class ReceiveData
{
    public byte[] data;

    public ReceiveData(byte[] data, int dataLength)
    {
        this.data = new byte[dataLength];
        Array.Copy(data, this.data, dataLength);

        Debug.Log("data length = " + this.data.Length);
    }
}