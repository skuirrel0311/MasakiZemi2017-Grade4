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
        client.SendBuffer("FuncA");
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

    //void SendInput()
    //{
    //    float velocity;
    //    velocity = 0.0f;
    //    if (leftScreenButton.IsPushing) velocity -= 1.0f;
    //    if (rightScreenButton.IsPushing) velocity += 1.0f;

    //    //どっちも押されていたら先に押されていた方を優先する
    //    if (leftScreenButton.IsPushing && rightScreenButton.IsPushing)
    //    {
    //        velocity = (leftScreenButton.pushingTime > rightScreenButton.pushingTime) ? -1.0f : 1.0f;
    //    }

    //    if (leftScreenButton.IsJustPush)
    //    {
    //        if (rightScreenButton.IsPushing)
    //        {
    //            client.SendBuffer(jumpActionMessgae);
    //        }
    //    }

    //    if (rightScreenButton.IsJustPush)
    //    {
    //        if (leftScreenButton.IsPushing)
    //        {
    //            client.SendBuffer(jumpActionMessgae);
    //        }
    //    }

    //    //値がないときは送らない
    //    if (velocity == 0.0f) return;
    //    client.SendBuffer(moveActionMessage);
    //    client.SendBuffer(BitConverter.GetBytes(velocity));
    //}
}
