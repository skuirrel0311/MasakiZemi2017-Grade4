using UnityEngine;

/// <summary>
/// 背景・小物などの動かないホログラム
/// </summary>
public class HoloObject : MonoBehaviour
{
    public enum HoloObjectType { Character, Item, Statics, Movable }
    public virtual HoloObjectType GetActorType { get { return HoloObjectType.Statics; } }

    Shader defaultShader;
    Shader grayScaleShader;

    /// <summary>
    /// ページが開かれた
    /// </summary>
    /// <param name="isFirst">そのページを開くのが初めてか？</param>
    public virtual void PageStart(int currentPageIndex, bool isFirst = true)
    {
        SetGrayScaleShader();
    }

    /// <summary>
    /// 動かすことのできないオブジェクトは灰色にする
    /// </summary>
    protected void SetGrayScaleShader()
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        grayScaleShader = MyAssetStore.I.GetAsset<Shader>("GrayScaleShader", "Shaders/");
        for (int i = 0; i < rends.Length; i++)
        {
            defaultShader = rends[i].material.shader;
            rends[i].material.shader = grayScaleShader;
        }
    }

    public void ResetShader()
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < rends.Length; i++)
        {
            if (GetActorType != HoloObjectType.Statics) continue;
            if (defaultShader == null) continue;
            rends[i].material.shader = defaultShader;
        }
    }

    /// <summary>
    /// ページが初めて開かれた時の場所に戻す
    /// </summary>
    public virtual void ResetTransform()
    {
        if (GetActorType == HoloObjectType.Statics) SetGrayScaleShader();
        gameObject.SetActive(true);
    }
    
    /// <summary>
    /// アイテムを持たせる
    /// </summary>
    public virtual void SetItem(GameObject item) { }
}
