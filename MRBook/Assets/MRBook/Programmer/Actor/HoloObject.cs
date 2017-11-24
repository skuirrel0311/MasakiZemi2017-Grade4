using UnityEngine;

/// <summary>
/// 背景・小物などの動かないホログラム
/// </summary>
public class HoloObject : MonoBehaviour
{
    public enum Type { Character, Item, Statics, Movable }
    public virtual Type GetActorType { get { return Type.Statics; } }

    /// <summary>
    /// そのオブジェクトが存在する（元の）ページのインデックス
    /// </summary>
    public int PageIndex { get; private set; }
    protected bool isFirst = true;

    public bool isMovable = false;
    public bool isFloating = false;

    AbstractHoloObjInputHandler inputHandler;
    AbstractHoloObjPassendObjBehaviour passendObjBehaviour;
    AbstractHoloObjResetter resetter;

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
        if (isMovable) inputHandler = new MovableObjInputHandler(this);
        else inputHandler = new StaticsObjInputHandler();
    }

    public virtual void PlayPage() { }

    /// <summary>
    /// ページが初めて開かれた時の場所に戻す
    /// </summary>
    public virtual void ResetTransform()
    {
        resetter.Reset();
        if (!gameObject.activeSelf) gameObject.SetActive(true);
    }

    public bool PassendObj(HoloObject obj)
    {
        return passendObjBehaviour.PassendObj(obj);
    }

    /// <summary>
    /// アイテムを持たせる
    /// </summary>
    public virtual void SetItem(GameObject item) { }
}
