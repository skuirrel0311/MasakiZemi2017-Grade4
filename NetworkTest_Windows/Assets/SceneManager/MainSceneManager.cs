using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : BaseManager<MainSceneManager>
{
    MyNetworkServer server;

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
    
    void Update()
    {
        //切断された
        if (server.state != BaseNetworkManager.NetworkState.Conecting)
        {
            if (isTransition) return;
            isTransition = true;
            LoadSceneManager.I.LoadScene("Title", true, 1.0f);
            return;
        }
    }
}
