using UnityEngine;

/// <summary>
/// アイテムの受け皿
/// </summary>
public class BaseItemSaucer : MonoBehaviour
{
    public virtual void SetItem(HoloItem item)
    {
    }

    public virtual bool CheckCanHaveItem(HoloItem item)
    {
        return false;
    }
}
