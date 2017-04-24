using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MatchingSceneManager : BaseManager<MatchingSceneManager>
{
    MyNetworkServer server;
    [SerializeField]
    Text serverIpText = null;

    [SerializeField]
    Text listenMessage = null;

    //接続中か？
    bool isConnecting = false;

    protected override void Start()
    {
        server = (MyNetworkServer)MyNetworkServer.I;
        serverIpText.text = server.serverIP;
        base.Start();
    }

    public void Join()
    {
        if (isConnecting) return;
        isConnecting = true;
        NotificationManager.I.PopUpMessage("接続待機中・・・", 30, 60.0f);

        MyNetworkClient.I.Initialize((isConnected) =>
        {
            if (isConnected)
            {
                NotificationManager.I.ClearMessage();
                LoadSceneManager.I.LoadScene("Main", true, 1.0f);
            }
            else
            {
                NotificationManager.I.PopUpMessage("接続に失敗しました", 30);
                isConnecting = false;
            }
        });
    }
}
