using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePage : MonoBehaviour
{
    /// <summary>
    /// お題
    /// </summary>
    public string missionText = "";

    [SerializeField]
    int pageIndex = 0;
    
    public RuntimeAnimatorController controller = null;

    //そのページを開くのが初めてか？
    bool isFirst = true;

    //ページに存在するアクター(ホログラムのオブジェクトのこと)のリスト
    public List<Actor> actorList = new List<Actor>();
    //ページに存在するアンカー(何かを発生させる位置のこと)のリスト
    public List<Transform> anchorList = new List<Transform>();

    //本の位置
    Vector3 instancePosition;
    Quaternion instanceRotation;

    Coroutine coroutine;

    //本の位置にページを固定する
    public void PageLock(Vector3 position, Quaternion rotation)
    {
        instancePosition = position;
        instanceRotation = rotation;
    }

    /// <summary>
    /// ページを開いた時に呼ぶ
    /// </summary>
    public void PageStart()
    {
        //todo:ナビメッシュを貼る

        if (!isFirst)
        {
            for (int i = 0; i < actorList.Count; i++)
            {
                actorList[i].PageStart(pageIndex, false);
            }
            return;
        }
        
        //初回のみリストに格納する
        isFirst = false;
        GameObject[] tempArray;
        tempArray = GameObject.FindGameObjectsWithTag("Actor");
        for (int i = 0; i < tempArray.Length; i++)
        {
            actorList.Add(tempArray[i].GetComponent<Actor>());
        }
        tempArray = GameObject.FindGameObjectsWithTag("Target");
        for (int i = 0; i < tempArray.Length; i++)
        {
            anchorList.Add(tempArray[i].transform);
        }

        for(int i = 0;i < actorList.Count;i++)
        {
            actorList[i].PageStart(pageIndex, true);
        }
    }

    //動かしたアクターを戻す
    public void ResetPage()
    {
        //後のページから戻ってきた場合はページ外にいるオブジェクトは戻さない
        for(int i = 0;i < actorList.Count;i++)
        {
            actorList[i].gameObject.SetActive(true);
            actorList[i].transform.position = actorList[i].firstPosition;
            actorList[i].transform.rotation = actorList[i].firstRotation;
        }
    }

    public Actor GetActor(string name)
    {
        for (int i = 0; i < actorList.Count; i++)
        {
            if (actorList[i].gameObject.name == name) return actorList[i];
        }
        return null;
    }
    public Transform GetAnchor(string name)
    {
        for (int i = 0; i < anchorList.Count; i++)
        {
            if (anchorList[i].name == name) return anchorList[i];
        }

        return null;
    }
    public void DisableActor(string name)
    {
        GetActor(name).gameObject.SetActive(false);
    }
}
