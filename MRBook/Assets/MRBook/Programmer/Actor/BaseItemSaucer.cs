using UnityEngine;

/// <summary>
/// アイテムの受け皿
/// </summary>
public class BaseItemSaucer : MonoBehaviour
{
    HoloObject owner;

    public virtual void Init(HoloObject owner)
    {
        this.owner = owner;
    }

    public virtual void SetItem(HoloItem item) { }
    public virtual bool CheckCanHaveItem(HoloItem item) { return false; }

    public virtual bool Equals(GameObject other)
    {
        return false;
    }
}
