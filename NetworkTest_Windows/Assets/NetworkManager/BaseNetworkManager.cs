using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Threading;

public class BaseNetworkManager : BaseManager<BaseNetworkManager>
{
    public enum NetworkState { Waiting, Conecting, Error, None }

    protected TcpClient client;
    protected Coroutine connectCoroutine;
    public volatile NetworkState state;
    protected NetworkStream stream;

    public string serverIP = "127.0.0.1";
    protected int portNum = 9000;

    protected List<ReceiveData> receiveList = new List<ReceiveData>();

    Thread receiveThread = null;

    protected override void Start()
    {
        state = NetworkState.None;
        receiveThread = new Thread(ReceiveWaiting);
        receiveThread.Start();
        base.Start();
    }

    protected string GetMyIPAddress()
    {
        string hostName = Dns.GetHostName();
        IPAddress[] addresses = Dns.GetHostAddresses(hostName);

        foreach (IPAddress add in addresses)
        {
            if (add.AddressFamily != AddressFamily.InterNetwork) continue;

            return add.ToString();
        }

        return "";
    }

    public virtual void Initialize(Action<bool> connecedCallback)
    {
        state = NetworkState.Waiting;
        connectCoroutine = StartCoroutine(WaitForConnect(connecedCallback));
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public virtual void Stop()
    {
        Debug.Log("終了します。");
        if (client != null)
        {
            client.Close();
        }
        state = NetworkState.None;
    }

    protected virtual IEnumerator WaitForConnect(Action<bool> connecedCallback)
    {
        yield break;
    }

    public void SendBuffer(string message)
    {
        if (stream == null) return;

        byte[] sendBuffer = System.Text.Encoding.Unicode.GetBytes(message);

        try
        {
            stream.Write(sendBuffer, 0, sendBuffer.Length);
            //強制書き出し
            stream.Flush();
        }
        catch
        {
            Stop();
        }
    }

    public void SendBuffer(byte[] message)
    {
        if (stream == null) return;

        try
        {
            stream.Write(message, 0, message.Length);
            //強制書き出し
            stream.Flush();
        }
        catch
        {
            Stop();
        }
    }

    void ReceiveBuffer(byte[] receiveData, int dataLength)
    {
        if (receiveData == null) return;
        Debug.Log("receive data now");
        receiveList.Add(new ReceiveData(receiveData, dataLength));
    }

    void ReceiveWaiting()
    {
        while (true)
        {
            if (state != NetworkState.Conecting)
            {
                Thread.Sleep(100);
                continue;
            }

            byte[] receiveData = new byte[2000];
            int dataLength = 0;
            try
            {
                dataLength = stream.Read(receiveData, 0, receiveData.Length);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Debug.Log("reading failure");
                Stop();
            }
            ReceiveBuffer(receiveData, dataLength);
        }
    }
}
