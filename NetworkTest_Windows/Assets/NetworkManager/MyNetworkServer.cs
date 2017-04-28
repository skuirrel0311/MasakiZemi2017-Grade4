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

    void Test()
    {
        int a = 10;
        byte[] b = BitConverter.GetBytes(a);
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
        RemoteCallRun();
    }

    //クライアントからの命令を実行します
    void RemoteCallRun()
    {
        if (state != NetworkState.Conecting) return;
        if (receiveList.Count == 0) return;

        //メソッドの名前は１番目のデータ
        string methodName = System.Text.Encoding.Unicode.GetString(receiveList[0].data);
        Debug.Log("call " + methodName);
        Debug.Log("receiveDataLength = " + receiveList.Count);

        if (receiveList.Count == 1)
        {
            //引数なしのメソッドの呼び出しがクライアントであった
            findRemoteCall(methodName).run.Invoke(null);
        }
        else if (receiveList.Count == 2)
        {
            //引数ありのメソッドの呼び出し 引数は２番目のデータに入っている
            findRemoteCall(methodName).run.Invoke(receiveList[1].data);
        }

        //リストは初期化
        for (int i = 0; i < receiveList.Count; i++)
        {
            receiveList[i] = null;
        }
        receiveList.Clear();
    }

    RemoteCall findRemoteCall(string methodName)
    {
        foreach (RemoteCall r in m_remoteCallList)
        {
            if (r.name == methodName.Split('_')[0]) return r;
        }

        //見つけられなかったら新しく作り、リストに追加
        RemoteCall remoteCall = new RemoteCall(methodName);
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

    public void FuncA(int hp)
    {
        NotificationManager.I.PopUpMessage("hp = " + hp);
    }

    //public void FuncA()
    //{
    //    NotificationManager.I.PopUpMessage("call FuncA");
    //}
}