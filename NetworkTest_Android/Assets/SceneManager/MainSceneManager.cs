using System;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : BaseManager<MainSceneManager>
{
    MyNetworkClient client;

    bool isTransition = false;

    [SerializeField]
    ScreenButton leftScreenButton = null;
    [SerializeField]
    ScreenButton rightScreenButton = null;

    protected override void Start()
    {
        client = (MyNetworkClient)MyNetworkClient.I;

        NotificationManager.I.PopUpMessage("接続完了");
        base.Start();
    }

    protected override void OnDestroy()
    {
        client.Stop();
        base.OnDestroy();
    }

    public void SendBuffer()
    {
        //client.RemoteCall("FuncA");
        client.RemoteCall("FuncA", "int", 5);
    }

    void Update()
    {
        if (client.state != BaseNetworkManager.NetworkState.Conecting)
        {
            if (isTransition) return;
            isTransition = true;
            LoadSceneManager.I.LoadScene("Title", true, 1.0f);
            return;
        }
    }
    
}
