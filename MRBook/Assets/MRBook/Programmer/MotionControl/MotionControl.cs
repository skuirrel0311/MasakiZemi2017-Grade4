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
    Maintenance //釣竿の手入れ
}

[System.Serializable]
public enum MotionType
{
    Normal,     //差分の無いユニークなモーション
    GetDrunk,   //酔っ払い時の差分があるモーション
    HasItem     //アイテムを持っている時の差分があるモーション
}

public static class MotionNameManager
{
    public static string GetMotionName(MotionName name, HoloMovableObject actor)
    {
        string motionName = name.ToString();

        if (actor == null) return motionName;

        if (actor.GetActorType != HoloObject.HoloObjectType.Character) return motionName;

        HoloCharacter character = (HoloCharacter)actor;

        motionName += character.hasItem_Right ? "_" + character.rightHandItemName : "";
        motionName += character.hasItem_Left ? "_" + character.leftHandItemName : "";

        if(character.IsGetAlcohol)
        {
            motionName = "GetDrunk_" + motionName;
        }

        return motionName;
    }
}