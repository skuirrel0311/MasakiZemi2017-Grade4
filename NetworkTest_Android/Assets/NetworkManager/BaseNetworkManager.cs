﻿using System;
using System.Collections;
using System.Net.Sockets;
using UnityEngine;
using System.Threading;

public class BaseNetworkManager : BaseManager<BaseNetworkManager>
{
    public enum NetworkState { Waiting, Conecting, Error , None}

    protected TcpClient client;
    protected Coroutine connectCoroutine;
    public volatile NetworkState state;
    protected NetworkStream stream;

    public string serverIP = "127.0.0.1";
    protected int portNum = 9000;

    public string receiveText = "";
    Thread receiveThread = null;

    protected override void Start()
    {
        state = NetworkState.None;
        receiveThread = new Thread(ReceiveWaiting);
        receiveThread.Start();
    }

    public virtual void Initialize()
    {
        state = NetworkState.Waiting;
        connectCoroutine = StartCoroutine(WaitForConnect());
    }

    protected override void OnDestroy()
    {
        Stop();
        base.OnDestroy();
    }

    public virtual void Stop()
    {
        if (client != null)
        {
            client.Close();
        }
        state = NetworkState.None;
    }

    protected virtual IEnumerator WaitForConnect()
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

    void ReceiveBuffer(byte[] receiveData, int dataLength)
    {
        if (receiveData == null) return;
        receiveText = System.Text.Encoding.Unicode.GetString(receiveData, 0, dataLength);
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
            catch(Exception ex)
            {
                Debug.Log(ex.Message);
                Stop();
            }
            ReceiveBuffer(receiveData, dataLength);
        }
    }
}
