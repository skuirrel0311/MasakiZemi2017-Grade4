using UnityEngine;

[System.Serializable]
public enum MotionName
{
    Wait,       //待機
    Walk,       //歩く
    Talk,       //話す
    Drink,      //飲む
    Eat,        //食べる
    Sit,        //座っている
    StandSit,   //立った状態から座るモーション
    Stand,      //立つ
    Swing,      //釣竿を振る
    Performance,//演奏する
    Dance,      //右側で踊る
    Lie,        //寝っ転がる
    Maintenance,//釣竿の手入れ
    Troubled,   //困惑する
    PutOff,     //引く
    Dead,       //死亡
    OpenDoorFailure,//ドア開け失敗
    FishingThrow,   //釣り始め
    FishingLoop,    //釣りループ
    FishingDead,    //釣りして引きずり込まれて死亡
    Abuse,      //いじめられる
    Stand_Fine, //勢いよく立つ
    SitPut,     //座ったまま物を置く
    StandPut,   //立ったまま物を置く
    Pass,       //アイテム渡す(料理以外)
    TakeBox,       //箱を受け取る
    nod,        //頷く
    LookInDead, //のぞき込んで死亡
    LookAround, //見回す
    DrinkP3,    //P3で飲む(酒瓶持ち)
    StandP3,    //P3で酔って立つ(酒瓶持ち)
    Appreciation,   //鑑賞する
    Applause,   //拍手
    Eat_Suffocation,//食べて喉詰まらせて死亡
    StandSitP3,  //P3で座る(酒瓶持ち)
    Guide,       //案内する
    SitSurprised,   //座ったまま驚く(死体発見時)、P5で兄ちゃんが驚く
    SitDelivery,    //座ったまま料理渡す
    Surprised,      //立ったまま驚く(死体発見時)
    StopBullying,   //いじめを止める
    OpenBox,    //箱を開ける
    LocateBullying, //いじめを見つける
    GetOnTurtle,    //亀に乗る
    GetOffTurtle,   //亀から降りる
    GetOffboat,     //船から降りる
    DropDownBoat,   //船から落ちて死ぬ
    Swim,       //泳ぐ
    Brutality,  //いじめる
    Pose,       //マッチョポーズ
    LieGetUp,   //寝転がりから起き上がる
    DanceLeft,  //左側で踊る
    TakeFood,   //食べ物を受け取る
    FrontSurprised, //乙姫 前向いて立ったまま驚く(P4浦島マッチョ化ルート用)
    LieNotice,  //寝ながらいじめ気付く→起きる
    WaveHand,   //手を振る
    QuickTalkTroubled,　//カメドン引き(困惑会話の会話部分消したの)
    RideTurtle_Loop,    //カメに乗ってる
    OldWait,    //爺用待機
    LookInBook, //本をのぞき込む(本の外で待機してる浦島用)
    Turn,        //振りむく(現地点ではP1浦島のお見送りのみ)
    StandSit_Fine,      //座る→食べる→立つ
    HangFrom,   //持たれてぶら下がる
    StandSit_Fine_Dead, //座る→食べる→死亡
    Brutality2, //いじめる２
    Brutality3,  //いじめる３
    LookInBookFishingrod
}

public static class MotionNameManager
{
    /// <summary>
    /// 持っているアイテムなどを考慮してモーションの名前を作る(itemSaucerのnullチェックは行わない)
    /// </summary>
    public static string GetMotionName(MotionName name, CharacterItemSaucer itemSaucer)
    {
        string motionName = name.ToString();

        motionName += itemSaucer.HasItem_Right ? "_" + itemSaucer.RightHandItem.GetName() : "";
        motionName += itemSaucer.HasItem_Left ? "_" + itemSaucer.LeftHandItem.GetName() : "";

        if (itemSaucer.IsGetAlcohol)
        {
            motionName = "GetDrunk_" + motionName;
        }
        Debug.Log(itemSaucer.name + " is call animation " + motionName);

        return motionName;
    }
}