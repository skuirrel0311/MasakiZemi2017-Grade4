using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ホログラムがずれた時になおす用（本番でも使う可能性あり）
/// </summary>
public class OffsetController : MonoBehaviour
{
    /// <summary>
    /// １回押すたびにずれる量
    /// </summary>
    public float movePower = 0.01f;

    public enum Direction { Up, Down}

    [SerializeField]
    AnchorPositionController anchor = null;
    [SerializeField]
    HoloButton[] buttons = null;
    
    public void MoveBook(int direction)
    {
        Vector3 moveVec = GetMoveVec(direction);

       // anchor.DeleteAnchor();

        anchor.transform.position += moveVec;

       // anchor.SaveAnchor();

        buttons[direction].Refresh();

        MainSceneManager gameManager = MainSceneManager.I;
        //if(gameManager.IsGameStart) gameManager.SetBookPositionByAnchor();
    }

    Vector3 GetMoveVec(int direction)
    {
        Vector3 vec = Vector3.zero;

        switch ((Direction)direction)
        {
            case Direction.Up:
                vec.y = movePower;
                break;
            case Direction.Down:
                vec.y = -movePower;
                break;
        }

        return vec;
    }
}
