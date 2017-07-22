using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// このプロジェクトにおいてはホログラム全般のこと
/// </summary>
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
    public virtual void PageStart(int currentPageIndex, bool isFirst = true)
    {
        if(isFirst)
        {
            pageIndex = currentPageIndex;
            firstPosition = transform.position;
            firstRotation = transform.rotation;
        }

        if (isMovable)
        {
            ActivateControl();
        }
        else
        {
            //Renderer[] rends = GetComponentsInChildren<Renderer>();
            //Shader grayScaleShader = AssetStoreManager.I.shaderStore.GetAsset("GrayScaleShader");
            //for (int i = 0; i < rends.Length; i++)
            //{
            //    //todo:シェーダーのパラメーターで切り替える
            //    //rends[i].material.shader = grayScaleShader;
            //}
        }
    }

    /// <summary>
    /// ページが初めて開かれた時の場所に戻す
    /// </summary>
    public virtual void ResetTransform()
    {
        gameObject.SetActive(true);
        transform.position = firstPosition;
        transform.rotation = firstRotation;

        //ほかのページに持っていけるオブジェクトの場合はグローバルになっている可能性がある
        if(isBring)
        {
            ActorManager.I.RemoveGlobal(this);
        }
    }

    void ActivateControl()
    {
        //操作できるようにする
        Debug.Log(gameObject.name + "は操作可能だ");
    }
}
