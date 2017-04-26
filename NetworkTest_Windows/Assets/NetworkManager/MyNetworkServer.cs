using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class MyNetworkServer : BaseNetworkManager
{
    TcpListener server;

    protected override void Start()
    {
        serverIP = GetMyIPAddress();

        if(serverIP == "")
        {
            Debug.LogError("自身のIPアドレスが取得できませんでした。");
        }

        server = new TcpListener(IPAddress.Parse(serverIP), portNum);

        base.Start();
    }

    string GetMyIPAddress()
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

    void Move(float velocty)
    {
        
    }

    public override void Initialize(Action<bool> connecedCallback)
    {
        if (state == NetworkState.Waiting || state == NetworkState.Conecting)
        {
            Debug.Log("接続中です");
            connecedCallback.Invoke(false);
            return;
        }
        server.Start();
        base.Initialize(connecedCallback);
    }

    void Update()
    {
        if (receiveList.Count == 0) return;

        string methodName = convertString(receiveList[0]);

        if (receiveList.Count == 1)
        {
            //引数なしのメソッドの呼び出しがクライアントであった
            RemoteCall task = new RemoteCall(methodName, this);
        }
        else
        {
            //引数ありは闇
            string model = convertString(receiveList[1]);

        }
    }

    string convertString(ReceiveData receiveData)
    {
        return BitConverter.ToString(receiveData.data);
    }

    IEnumerator Listen()
    {
        float t = 0.0f;
        float limit = 60.0f;
        while (true)
        {
            if (server.Pending()) break;
            t += Time.deltaTime;

            if (t > limit)
            {
                Debug.Log("タイムアウト");
                yield break;
            }
            yield return null;
        }

        try
        {
            client = server.AcceptTcpClient();
            stream = client.GetStream();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.Log("接続に失敗しました。");
            yield break;
        }
    }

    protected override IEnumerator WaitForConnect(Action<bool> connecedCallback)
    {
        Coroutine coroutine = StartCoroutine(Listen());

        yield return coroutine;

        //接続に失敗した
        if (stream == null)
        {
            state = NetworkState.Error;
            Stop();
            connecedCallback.Invoke(false);
            yield break;
        }

        state = NetworkState.Conecting;
        connecedCallback.Invoke(true);
        Debug.Log("接続完了");
    }

    public override void Stop()
    {
        if (server != null)
        {
            server.Stop();
        }
        base.Stop();
        stream = null;
    }
}