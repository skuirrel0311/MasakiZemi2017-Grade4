using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MatchingSceneManager : BaseManager<MatchingSceneManager>
{
    MyNetworkClient client;
    [SerializeField]
    InputField serverIpInput;

    //接続中か？
    bool isConnecting = false;

    public void ChangeServerIp()
    {
        client = (MyNetworkClient)MyNetworkClient.I;
        client.serverIP = serverIpInput.text;
    }

    public void Join()
    {
        if (isConnecting) return;

        isConnecting = true;
        NotificationManager.I.PopUpMessage("接続中・・・", 30, 30.0f);
        MyNetworkClient.I.Initialize((isConnected) =>
        {
            if (isConnected)
            {
                NotificationManager.I.ClearMessage();
                LoadSceneManager.I.LoadScene("Main", true, 1.0f);
            }
            else
            {
                NotificationManager.I.PopUpMessage("接続に失敗しました");
            }
        });

    }
}
