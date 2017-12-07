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
    Dance,      //踊る
    Lie,        //寝っ転がる
    Maintenance,//釣竿の手入れ
    Troubled,   //困惑する
    PutOff,     //引く
    Dead,       //死亡
    OpenDoorFailure,//ドア開け失敗
    FishingThrow,   //釣り始め
    FishingLoop,    //釣りループ
    FishingDead,    //釣りして引きずり込まれて死亡
    Abuse       //いじめられる
}

public static class MotionNameManager
{
    /// <summary>
    /// 持っているアイテムなどを考慮してモーションの名前を作る(itemSaucerのnullチェックは行わない)
    /// </summary>
    public static string GetMotionName(MotionName name, CharacterItemSaucer itemSaucer)
    {
        string motionName = name.ToString();

        motionName += itemSaucer.HasItem_Right ? "_" + itemSaucer.RightHandItem.name : "";
        motionName += itemSaucer.HasItem_Left ? "_" + itemSaucer.LeftHandItem.name : "";

        if (itemSaucer.IsGetAlcohol)
        {
            motionName = "GetDrunk_" + motionName;
        }
        Debug.Log(itemSaucer.name + " is call animation " + motionName);

        return motionName;
    }
}