using UnityEngine;

[System.Serializable]
public enum MotionName
{
    Wait,       //待機
    Walk,       //歩く
    Talk,       //話す
    Drink,      //飲む
    Eat,        //食べる
    Sit,        //座る
    Stand,      //立つ
    Swing,      //釣竿を振る
    Performance,//演奏する
    Dance,      //踊る
    Lie,        //寝っ転がる
    Maintenance,//釣竿の手入れ
    Troubled,   //困惑する
    PutOff      //引く
}

public static class MotionNameManager
{
    public static string GetMotionName(MotionName name, HoloMovableObject actor)
    {
        string motionName = name.ToString();

        if (actor == null || actor.GetActorType != HoloObject.HoloObjectType.Character)
        {
            Debug.Log(actor.name + "is call animation " + motionName);
            return motionName;
        }

        HoloCharacter character = (HoloCharacter)actor;

        motionName += character.hasItem_Right ? "_" + character.rightHandItem.name : "";
        motionName += character.hasItem_Left ? "_" + character.leftHandItem.name : "";

        if(character.IsGetAlcohol)
        {
            motionName = "GetDrunk_" + motionName;
        }
        Debug.Log(actor.name + "is call animation " + motionName);

        return motionName;
    }
}