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
        listenMessage.gameObject.SetActive(true);
        listenMessage.text = "接続待機中";
        MyNetworkClient.I.Initialize((isConnected)=>
        {
            if (isConnected) LoadSceneManager.I.LoadScene("Main", true, 1.0f);
            else
            {
                listenMessage.text = "接続に失敗しました";
                StartCoroutine(KKUtilities.Delay(1.0f, () =>
                {
                    listenMessage.gameObject.SetActive(false);
                }));
            }
        });
    }
}
