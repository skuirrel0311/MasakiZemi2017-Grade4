using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MatchingSceneManager : BaseManager<MatchingSceneManager>
{
    MyNetworkClient client;
    [SerializeField]
    InputField serverIpInput;

    bool isConnecting = false;

    public void ChangeServerIp()
    {
        client = (MyNetworkClient)MyNetworkClient.I;
        client.serverIP = serverIpInput.text;
    }

    void Update()
    {
        if (!isConnecting) return;

        if(client.state == BaseNetworkManager.NetworkState.Conecting)
        {
            LoadSceneManager.I.LoadScene("Main", true, 1.0f);
        }
    }

    public void Join()
    {
        if (isConnecting) return;
        MyNetworkClient.I.Initialize();
    }
}
