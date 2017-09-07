using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasePage : MonoBehaviour
{
    /// <summary>
    /// お題
    /// </summary>
    public string missionText = "";

    int pageIndex = 0;

    public RuntimeAnimatorController controller = null;

    /// <summary>
    /// オクルージョン用のオブジェクト
    /// </summary>
    [SerializeField]
    GameObject[] bookObjects = null;

    //ページに存在するホログラム
    public Dictionary<string, HoloObject> objectDictionary = new Dictionary<string, HoloObject>();
    //ページに存在するアンカー(何かを発生させる位置のこと)のリスト
    public Dictionary<string, Transform> targetPointDictionary = new Dictionary<string, Transform>();

    public NavMeshAgent[] agents = null;
    
    /// <summary>
    /// 本の位置にページを固定する
    /// </summary>
    public void PageLock(Vector3 position, Quaternion rotation, int pageIndex)
    {
        transform.position = position;
        transform.rotation = rotation;
        this.pageIndex = pageIndex;
    }

    /// <summary>
    /// ページを開いた時に呼ぶ
    /// </summary>
    public void PageStart(bool isFirst)
    {
        Debug.Log("is first = " + isFirst  + " page index = " + pageIndex);
        if (!isFirst)
        {
            foreach (string key in objectDictionary.Keys)
            {
                objectDictionary[key].PageStart(pageIndex, true);
            }
            return;
        }

        //初回のみリストに格納する
        GameObject[] tempArray;
        
        tempArray = GameObject.FindGameObjectsWithTag("Actor");
        for (int i = 0; i < tempArray.Length; i++)
        {
            HoloObject actor = tempArray[i].GetComponent<HoloObject>();
            if (actor != null)
            {
                Debug.Log("add actor " + actor.name);
                objectDictionary.Add(actor.name, actor);
            }
        }
        
        tempArray = GameObject.FindGameObjectsWithTag("Target");
        for (int i = 0; i < tempArray.Length; i++)
        {
            Debug.Log("add target point " + tempArray[i].name);
            targetPointDictionary.Add(tempArray[i].name, tempArray[i].transform);
        }
        Material visibleMat = MainSceneManager.I.visibleMat;
        for (int i = 0; i < bookObjects.Length; i++)
        {
            bookObjects[i].GetComponent<Renderer>().material = visibleMat;
        }

        foreach (string key in objectDictionary.Keys)
        {
            objectDictionary[key].PageStart(pageIndex, true);
        }
    }

    public void PlayPage()
    {
        foreach (string key in objectDictionary.Keys)
        {
            objectDictionary[key].ResetShader();
        }
    }

    /// <summary>
    /// このページに登録されているアクターをページを開いた時の位置に戻す
    /// </summary>
    public void ResetPage()
    {
        foreach (string key in objectDictionary.Keys)
        {
            objectDictionary[key].ResetTransform();
        }
        //todo:リセット中のアニメーション
    }
}
