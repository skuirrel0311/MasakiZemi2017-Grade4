using UnityEngine;

/// <summary>
/// 背景・小物などの動かないホログラム
/// </summary>
public class HoloObject : MonoBehaviour
{
    public enum Type { Character, Item, Statics, Movable }
    public virtual Type GetActorType { get { return Type.Statics; } }

    Shader defaultShader;
    Shader grayScaleShader;

    /// <summary>
    /// そのオブジェクトが存在する（元の）ページのインデックス
    /// </summary>
    public int PageIndex { get; private set; }
    protected bool isFirst = true;

    /// <summary>
    /// ページが開かれた
    /// </summary>
    /// <param name="isFirst">そのページを開くのが初めてか？</param>
    public virtual void PageStart(int currentPageIndex)
    {
        if(isFirst)
        {
            PageIndex = currentPageIndex;
        }
        isFirst = false;
    }

    public virtual void PlayPage() { }

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
            if (defaultShader == null) continue;
            rends[i].material.shader = defaultShader;
        }
    }

    /// <summary>
    /// ページが初めて開かれた時の場所に戻す
    /// </summary>
    public virtual void ResetTransform()
    {
        if(!gameObject.activeSelf) gameObject.SetActive(true);
    }
    
    /// <summary>
    /// アイテムを持たせる
    /// </summary>
    public virtual void SetItem(GameObject item) { }
}
