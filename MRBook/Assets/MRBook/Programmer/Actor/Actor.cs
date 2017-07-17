using UnityEngine;

public class Actor : MonoBehaviour
{
    /// <summary>
    /// 動かせるか
    /// </summary>
    public bool isMovable = false;
    /// <summary>
    /// 別のページに持っていけるか
    /// </summary>
    public bool isBring = false;
    /// <summary>
    /// アイテムを持たせることができるか
    /// </summary>
    public bool CanHaveItem = false;
    /// <summary>
    /// 持たせることができるか
    /// </summary>
    public bool IsItem = false;

    /// <summary>
    /// そのオブジェクトが存在する（元の）ページのインデックス
    /// </summary>
    public int pageIndex { get; private set; }

    //初期値
    public Vector3 firstPosition { get; private set; }
    public Quaternion firstRotation { get; private set; }
    
    /// <summary>
    /// ページが開かれた
    /// </summary>
    /// <param name="isFirst">そのページを開くのが初めてか？</param>
    public void PageStart(int currentPageIndex, bool isFirst = true)
    {
        if(isFirst)
        {
            pageIndex = currentPageIndex;
            firstPosition = transform.position;
            firstRotation = transform.rotation;
        }

        if(pageIndex == currentPageIndex)
        {
            if(isFirst && isMovable)
            {
                ActivateControl();
            }
            if(!isFirst && isBring)
            {
                ActivateControl();
            }
        }
        else
        {
            //違うページ(必ず動かせる)
            ActivateControl();
        }
    }

    /// <summary>
    /// ページが初めて開かれた時の場所に戻す
    /// </summary>
    public void ResetTransform()
    {
        gameObject.SetActive(true);
        transform.position = firstPosition;
        transform.rotation = firstRotation;
    }

    void ActivateControl()
    {
        //操作できるようにする
        Debug.Log(gameObject.name + "は操作可能だ");
    }
}
