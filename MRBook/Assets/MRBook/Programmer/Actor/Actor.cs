using UnityEngine;

public class Actor : MonoBehaviour
{
    /// <summary>
    /// 動かせるか
    /// </summary>
    [SerializeField]
    bool isMovable = false;
    /// <summary>
    /// 別のページに持っていけるか
    /// </summary>
    [SerializeField]
    bool isBring = false;

    /// <summary>
    /// そのオブジェクトが存在する（元の）ページのインデックス
    /// </summary>
    int pageIndex = 0;

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

    void ActivateControl()
    {
        //操作できるようにする
        Debug.Log(gameObject.name + "は操作可能だ");
    }
}
