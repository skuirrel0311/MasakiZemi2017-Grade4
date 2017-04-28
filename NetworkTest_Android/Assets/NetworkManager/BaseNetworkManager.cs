using System;
using System.Collections;
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

    public string receiveText = "";
    Thread receiveThread = null;

    protected override void Start()
    {
        state = NetworkState.None;
        receiveThread = new Thread(ReceiveWaiting);
        receiveThread.Start();
        base.Start();
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
    
    /// <summary>
    /// 引数なしのメソッドの呼び出し
    /// </summary>
    public void RemoteCall(string methodName)
    {
        if (stream == null) return;

        byte[] sendBuffer = System.Text.Encoding.Unicode.GetBytes(methodName);

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
    
    /// <summary>
    /// 引数ありのメソッドの呼び出し
    /// </summary>
    /// <param name="type">引数の型</param>
    public void RemoteCall<T>(string methodName, string type, T data)
    {
        if (stream == null) return;
        try
        {
            byte[] sendBuffer = encodeData("string", methodName);
            stream.Write(sendBuffer, 0, sendBuffer.Length);
            stream.Flush();

            sendBuffer = encodeData(type, data);
            stream.Write(sendBuffer, 0, sendBuffer.Length);
            stream.Flush();
        }
        catch
        {
            Stop();
        }
    }

    byte[] encodeData<T>(string type, T data)
    {
        if (type == "int")
        {
            return BitConverter.GetBytes((int)(object)data);
        }
        if (type == "float")
        {
            return BitConverter.GetBytes((float)(object)data);
        }
        if (type == "bool")
        {
            return BitConverter.GetBytes((bool)(object)data);
        }
        if (type == "string")
        {
            return System.Text.Encoding.Unicode.GetBytes((string)(object)data);
        }
        if (type == "double")
        {
            return BitConverter.GetBytes((bool)(object)data);
        }

        return null;
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
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                Stop();
            }
            ReceiveBuffer(receiveData, dataLength);
        }
    }
}
