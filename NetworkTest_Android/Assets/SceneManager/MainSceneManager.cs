using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : BaseManager<MainSceneManager>
{
    MyNetworkClient client;
    [SerializeField]
    InputField sendText = null;

    [SerializeField]
    Text receiveText = null;

    [SerializeField]
    Text stateText = null;

    protected override void Start()
    {
        client = (MyNetworkClient)MyNetworkClient.I;
        base.Start();
    }

    public void SendMessage()
    {
        if (client.state != BaseNetworkManager.NetworkState.Conecting) return;
        client.SendBuffer(sendText.text);
    }

    void Update()
    {
        stateText.text = client.state.ToString();
        if (client.state != BaseNetworkManager.NetworkState.Conecting)
        {
            LoadSceneManager.I.LoadScene("Title", true, 1.0f);
            return;
        }
        receiveText.text = client.receiveText;

    }
}
