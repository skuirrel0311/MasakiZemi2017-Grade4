using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//実行中にモデルが変化することがあるオブジェクト
public class ChangeModelObject : MonoBehaviour
{
    [SerializeField]
    GameObject original = null;

    //差し替えるモデル
    [SerializeField]
    GameObject replacementObject = null;

    bool isOriginal = true;

    public void ChangeModel()
    {
        isOriginal = !isOriginal;
        //差し替え
        ParticleManager.I.Play("Doron", transform.position, Quaternion.identity);
        original.SetActive(isOriginal);
        replacementObject.SetActive(!isOriginal);
    }
}
