using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 背景・小物などの動かないホログラム
/// </summary>
public class HoloObject : MonoBehaviour
{
    public enum Type { Character, Item, Statics }
    public virtual Type GetActorType { get { return Type.Statics; } }

    /// <summary>
    /// そのオブジェクトが存在する（元の）ページのインデックス
    /// </summary>
    public int PageIndex { get; private set; }
    bool isFirst = true;

    [SerializeField]
    bool isMovable = false;
    [SerializeField]
    bool isFloating = false;

    public bool IsMovable { get { return isMovable; } }
    public bool IsFloating { get { return isFloating; } }

    public AbstractHoloObjInputHandler inputHandler { get; protected set; }
    public BaseHoloObjResetter resetter { get; protected set; }

    /// <summary>
    /// ページが開かれた
    /// </summary>
    /// <param name="isFirst">そのページを開くのが初めてか？</param>
    public virtual void PageStart(int currentPageIndex)
    {
        if (isFirst)
        {
            PageIndex = currentPageIndex;
            Init();
        }
        isFirst = false;
    }

    protected virtual void Init()
    {
        InitInputHandler();
        InitResetter();
    }

    protected virtual void InitInputHandler()
    {
        //HoloObjectはそもそもタップに対して応答しない
        inputHandler = null;
    }

    protected virtual void InitResetter()
    {
        resetter = new HoloObjResetter(this);
    }

    public virtual void PlayPage() { }
}
