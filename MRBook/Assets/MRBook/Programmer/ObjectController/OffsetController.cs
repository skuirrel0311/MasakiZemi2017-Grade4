using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ホログラムがずれた時になおす用（本番でも使う可能性あり）
/// </summary>
public class OffsetController : MonoBehaviour
{
    public enum Direction { Up, Down, Left, Right, Front, Back}

    /// <summary>
    /// １回押すたびにずれる量
    /// </summary>
    [SerializeField]
    float movePower = 0.01f;
    
    [SerializeField]
    HoloButton[] buttons = null;

    Vector3 zeroVec = Vector3.zero;

    bool isCalc = true;

    List<HoloObject> objList;

    void Start()
    {
        MainSceneManager.I.OnPageChanged += (o, n) => isCalc = true;
    }
    
    //別にWorldAnchorの位置を操作するわけではない
    public void MoveBook(int direction)
    {
        Vector3 moveVec = GetMoveVec(direction);
        buttons[direction].Refresh();

        //ActorについているNavMeshAgentはすべて外しておかなければならない
        if(isCalc)
        {
            isCalc = false;
            objList = ActorManager.I.GetAllObject();
            //多少GCは吐くがページごとに1回なので気にしなくてもよいはず…
            objList.RemoveAll(n => n.GetActorType == HoloObject.HoloObjectType.Statics);
        }

        for(int i = 0;i < objList.Count;i++)
        {
        }
    }

    Vector3 GetMoveVec(int direction)
    {
        Vector3 vec = zeroVec;
        
        switch ((Direction)direction)
        {
            case Direction.Up:
                vec.y = movePower;
                break;
            case Direction.Down:
                vec.y = -movePower;
                break;
            case Direction.Left:
                vec.x = -movePower;
                break;
            case Direction.Right:
                vec.x = movePower;
                break;
            case Direction.Front:
                vec.z = movePower;
                break;
            case Direction.Back:
                vec.z = -movePower;
                break;
        }

        return vec;
    }
}
