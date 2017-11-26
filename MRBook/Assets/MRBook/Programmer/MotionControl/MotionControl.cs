using UnityEngine;

[System.Serializable]
public enum MotionName
{
    Wait,               //待機
    Walk,               //歩く
    Talk,               //話す
    Drink,              //飲む
    Eat,                //食べる
    Sit,                //座っている
    StandSit,           //立った状態から座るモーション
    Stand,              //立つ
    Swing,              //釣竿を振る
    Performance,        //演奏する
    Dance,              //踊る
    Lie,                //寝っ転がる
    Maintenance,        //釣竿の手入れ
    Troubled,           //困惑する
    PutOff,             //引く
    Dead,               //死亡
    OpenDoorFailure,    //ドア開け失敗
    Peck                //スズメが突くモーション
}

public static class MotionNameManager
{
    public static string GetMotionName(MotionName name, HandCharacter handCharacter)
    {
        string motionName = name.ToString();
        
        if (handCharacter == null || handCharacter.GetActorType != HoloObject.Type.Character)
        {
            Debug.Log(handCharacter.name + "is call animation " + motionName);
            return motionName;
        }
        
        motionName += handCharacter.HasItem_Right ? "_" + handCharacter.RightHandItem.name : "";
        motionName += handCharacter.HasItem_Left ? "_" + handCharacter.LeftHandItem.name : "";

        if(handCharacter.IsGetAlcohol)
        {
            motionName = "GetDrunk_" + motionName;
        }
        Debug.Log(handCharacter.name + "is call animation " + motionName);

        return motionName;
    }
}