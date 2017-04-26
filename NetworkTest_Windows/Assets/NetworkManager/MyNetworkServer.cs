using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;

public class MyNetworkServer : BaseNetworkManager
{
    TcpListener server;
    List<RemoteCall> m_remoteCallList = new List<RemoteCall>();

    protected override void Start()
    {
        serverIP = GetMyIPAddress();

        if (serverIP == "")
        {
            Debug.LogError("自身のIPアドレスが取得できませんでした。");
        }

        server = new TcpListener(IPAddress.Parse(serverIP), portNum);
        RemoteCall.Initialize(this);

        base.Start();
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

        //メソッドの名前は１番目のデータ
        string methodName = System.Text.Encoding.Unicode.GetString(receiveList[0].data);
        Debug.Log("call " + methodName);

        if (receiveList.Count == 1)
        {
            //引数なしのメソッドの呼び出しがクライアントであった
            findRemoteCall(methodName).run.Invoke(null);
        }
        else if(receiveList.Count == 3)
        {
            //引数の型は２番目のデータ
            string type = BitConverter.ToString(receiveList[1].data);
            //引数の値は３番目のデータ
            findRemoteCall(methodName, type).run.Invoke(receiveList[2].data);
        }

        //リストは初期化
        for(int i = 0;i< receiveList.Count;i++)
        {
            receiveList[i] = null;
        }
        receiveList.Clear();
    }

    RemoteCall findRemoteCall(string methodName, string type = "")
    {
        foreach (RemoteCall r in m_remoteCallList)
        {
            if (r.name == methodName) return r;
        }

        //見つけられなかったら新しく作り、リストに追加
        RemoteCall remoteCall = new RemoteCall(name, type);
        m_remoteCallList.Add(remoteCall);

        return remoteCall;
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

    void FuncA()
    {
        NotificationManager.I.PopUpMessage("call FuncA");
    }
}