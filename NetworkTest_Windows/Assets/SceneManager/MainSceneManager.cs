using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : BaseManager<MainSceneManager>
{
    MyNetworkServer server;
    [SerializeField]
    InputField sendText = null;

    [SerializeField]
    Text[] receiveText = null;

    [SerializeField]
    Text stateText = null;

    bool isTransition = false;

    protected override void Start()
    {
        server = (MyNetworkServer)MyNetworkServer.I;

        NotificationManager.I.PopUpMessage("接続完了");
        base.Start();
    }

    protected override void OnDestroy()
    {
        server.Stop();
        base.OnDestroy();
    }

    public void SendBuffer()
    {
        if (server.state != BaseNetworkManager.NetworkState.Conecting) return;
        server.SendBuffer(sendText.text);
    }

    void Update()
    {
        stateText.text = server.state.ToString();
        if (server.state != BaseNetworkManager.NetworkState.Conecting)
        {
            if (isTransition) return;
            isTransition = true;
            LoadSceneManager.I.LoadScene("Title", true, 1.0f);
            return;
        }
        //if (server.receiveTextList.Count != 0)
        //{
        //    SetReceiveText();

        //    server.receiveTextList.Clear();
        //}

    }

    void SetReceiveText()
    {
        //if(server.receiveTextList.Count == 1)
        //{
        //    //単一の命令
        //    receiveText[1].text = receiveText[0].text;
        //    receiveText[0].text = server.receiveTextList[0];
        //}
        //else
        //{
        //    //複数の命令が来た(とりあえず２つより大きいことは考えない)
        //    receiveText[0].text = server.receiveTextList[0];
        //    receiveText[1].text = server.receiveTextList[1];
        //}
    }
}
