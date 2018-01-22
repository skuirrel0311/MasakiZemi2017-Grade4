using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ホログラムがずれた時になおす用（本番でも使う可能性あり）
/// </summary>
public class OffsetController : BaseManager<OffsetController>
{
    public enum Direction { Up, Down, Left, Right, Front, Back }

    /// <summary>
    /// １回押すたびにずれる量
    /// </summary>
    [SerializeField]
    float movePower = 0.01f;

    Vector3 zeroVec = Vector3.zero;
    
    public Transform bookTransform = null;

    protected override void Start()
    {
        base.Start();

        bookTransform = BookPositionModifier.I.bookTransform;
    }

    //別にWorldAnchorの位置を操作するわけではない
    public void MoveBook(int direction)
    {
        Vector3 moveVec = GetMoveVec(direction);
        
        bookTransform.position += moveVec;
        BookPositionModifier.I.ModifyBookPosition(false);
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
