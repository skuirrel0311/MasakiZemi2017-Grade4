using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : BaseManager<MainSceneManager>
{
    MyNetworkServer server;
    [SerializeField]
    InputField sendText = null;

    [SerializeField]
    Text receiveText = null;

    [SerializeField]
    Text stateText = null;

    protected override void Start()
    {
        server = (MyNetworkServer)MyNetworkServer.I;
        base.Start();
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
            LoadSceneManager.I.LoadScene("Title", true, 1.0f);
            return;
        }
        receiveText.text = server.receiveText;

    }
}
