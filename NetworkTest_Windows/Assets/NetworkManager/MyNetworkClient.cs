using System;
using System.Collections;
using System.Net.Sockets;
using UnityEngine;

public class MyNetworkClient : BaseNetworkManager
{
    protected override void Start()
    {
        client = new TcpClient();
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
        base.Initialize(connecedCallback);
    }

    protected override IEnumerator WaitForConnect(Action<bool> connecedCallback)
    {
        //tryNum回
        int tryNum = 10;
        //interval秒ごとに接続し失敗したら接続失敗とする
        WaitForSeconds interval = new WaitForSeconds(0.5f);
        
        while (true)
        {
            if (Connect()) break;

            tryNum--;
            if (tryNum <= 0)
            {
                Debug.Log("接続失敗");
                state = NetworkState.Error;
                connecedCallback.Invoke(false);
                break;
            }
            yield return interval;
        }

        Debug.Log("接続成功");
        connecedCallback.Invoke(true);
        state = NetworkState.Conecting;
    }

    bool Connect()
    {
        try
        {
            Debug.Log("serverIP = " + serverIP);
            client.Connect(serverIP, portNum);
            stream = client.GetStream();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return false;
        }

        return true;
    }
}
