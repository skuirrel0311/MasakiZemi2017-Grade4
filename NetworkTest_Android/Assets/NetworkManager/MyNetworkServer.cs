using System;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class MyNetworkServer : BaseNetworkManager
{
    TcpListener server;

    protected override void Start()
    {
        //192.168.0.7
        string hostName = Dns.GetHostName();
        IPAddress[] addresses = Dns.GetHostAddresses(hostName);

        foreach(IPAddress add in addresses)
        {
            if (add.AddressFamily != AddressFamily.InterNetwork) continue;
            serverIP = add.ToString();
        }


        server = new TcpListener(System.Net.IPAddress.Parse(serverIP), portNum);
        
        base.Start();
    }

    public override void Initialize()
    {
        if (state == NetworkState.Waiting || state == NetworkState.Conecting)
        {
            Debug.Log("接続中です");
            return;
        }
        server.Start();
        base.Initialize();
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

    protected override IEnumerator WaitForConnect()
    {
        Coroutine coroutine = StartCoroutine(Listen());

        yield return coroutine;

        //接続に失敗した
        if (stream == null)
        {
            state = NetworkState.Error;
            yield break;
        }

        state = NetworkState.Conecting;
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