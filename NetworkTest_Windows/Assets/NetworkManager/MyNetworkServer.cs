using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class MyNetworkServer : BaseNetworkManager
{
    TcpListener server;
    List<RCVMethod> rcvMethodList = new List<RCVMethod>();

    protected override void Start()
    {
        string hostName = Dns.GetHostName();
        IPAddress[] addresses = Dns.GetHostAddresses(hostName);

        foreach(IPAddress add in addresses)
        {
            if (add.AddressFamily != AddressFamily.InterNetwork) continue;
            serverIP = add.ToString();
        }
        
        server = new TcpListener(IPAddress.Parse(serverIP), portNum);

        rcvMethodList.Add(new RCVMethod((data) =>
        {
            float v = BitConverter.ToSingle(data, 0);
            Move(v);
        },
        "move"));

        base.Start();
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
        /*
         * if(rec[0] == "a")
         * {
         *      float v = rec[1];
         *      move(v);
         * }
         */
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