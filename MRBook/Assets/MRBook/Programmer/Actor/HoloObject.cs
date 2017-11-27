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
    [SerializeField]
    bool canHaveItem = false;
    [SerializeField]
    BaseObjInputHandler inputHandler = null;
    [SerializeField]
    BaseItemSaucer itemSaucer = null;
    [SerializeField]
    int inPlayLayer = 4;

    public bool IsMovable { get { return isMovable; } }
    public bool IsFloating { get { return isFloating; } }
    public bool CanHaveItem { get { return canHaveItem; } }
    public BaseObjInputHandler InputHandler { get { return inputHandler; } }
    public BaseItemSaucer ItemSaucer { get { return itemSaucer; } }

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
        InitResetter();
    }

    protected virtual void InitResetter()
    {
        resetter = new HoloObjResetter(this);
    }

    public virtual void PlayPage()
    {
        gameObject.layer = inPlayLayer;
    }

    public bool CheckCanHaveItem(HoloItem item)
    {
        if (ItemSaucer == null) return false;

        return ItemSaucer.CheckCanHaveItem(item);
    }

    public void SetItem(HoloItem item)
    {
        if (!CheckCanHaveItem(item)) return;

        //CheckCanHaveItemがtrueになったということはitemSaucerはnullではない
        ItemSaucer.SetItem(item);
    }
    
    public override bool Equals(object other)
    {
        return gameObject.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
