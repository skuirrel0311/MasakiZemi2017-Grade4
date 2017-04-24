using System;
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

    bool isTransition = false;

    [SerializeField]
    ScreenButton leftScreenButton = null;
    [SerializeField]
    ScreenButton rightScreenButton = null;

    byte[] moveActionMessage;
    byte[] jumpActionMessgae;

    protected override void Start()
    {
        client = (MyNetworkClient)MyNetworkClient.I;

        NotificationManager.I.PopUpMessage("接続完了");

        //あらかじめ命令をByteに変換しておく
        moveActionMessage = System.Text.Encoding.Unicode.GetBytes("move");
        jumpActionMessgae = System.Text.Encoding.Unicode.GetBytes("jumped");

        Debug.Log("move = " + moveActionMessage.Length);
        Debug.Log("jumped = " + jumpActionMessgae.Length);
        base.Start();
    }

    protected override void OnDestroy()
    {
        client.Stop();
        base.OnDestroy();
    }

    public void SendBuffer()
    {
        client.SendBuffer("move");
        client.SendBuffer(BitConverter.GetBytes(1.0f));
    }

    void Update()
    {
        stateText.text = client.state.ToString();

        //SendInput();
        if (client.state != BaseNetworkManager.NetworkState.Conecting)
        {
            if (isTransition) return;
            isTransition = true;
            LoadSceneManager.I.LoadScene("Title", true, 1.0f);
            return;
        }
    }

    void SendInput()
    {
        float velocity;
        velocity = 0.0f;
        if (leftScreenButton.IsPushing) velocity -= 1.0f;
        if (rightScreenButton.IsPushing) velocity += 1.0f;

        //どっちも押されていたら先に押されていた方を優先する
        if (leftScreenButton.IsPushing && rightScreenButton.IsPushing)
        {
            velocity = (leftScreenButton.pushingTime > rightScreenButton.pushingTime) ? -1.0f : 1.0f;
        }

        if (leftScreenButton.IsJustPush)
        {
            if (rightScreenButton.IsPushing)
            {
                client.SendBuffer(jumpActionMessgae);
            }
        }

        if (rightScreenButton.IsJustPush)
        {
            if (leftScreenButton.IsPushing)
            {
                client.SendBuffer(jumpActionMessgae);
            }
        }

        //値がないときは送らない
        if (velocity == 0.0f) return;
        client.SendBuffer(moveActionMessage);
        client.SendBuffer(BitConverter.GetBytes(velocity));
    }
}
