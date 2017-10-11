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


    //ページに存在するアンカー(何かを発生させる位置のこと)のリスト
    public Dictionary<string, Transform> targetPointDictionary = new Dictionary<string, Transform>();

    //同名のオブジェクトがあるときには使えない
    public Dictionary<string, HoloObject> objectDictionary = new Dictionary<string, HoloObject>();
    //ページに存在するホログラム全部。(灰色にさせたりするときに必要)
    public List<HoloObject> objectList = new List<HoloObject>();
    //動かせるホログラム全部
    public Dictionary<string, HoloMovableObject> movableObjectDictionary = new Dictionary<string, HoloMovableObject>();
    //よく使うのでリストでも管理
    public List<HoloMovableObject> movableObjectList = new List<HoloMovableObject>();

    /// <summary>
    /// 本の位置にページを固定する
    /// </summary>
    public void PageLock(Vector3 position, Quaternion rotation, int pageIndex)
    {
        transform.SetPositionAndRotation(position, rotation);
        for (int i = 0; i < movableObjectList.Count; i++)
        {
            movableObjectList[i].ApplyDefaultTransform();
        }
        this.pageIndex = pageIndex;
    }

    /// <summary>
    /// ページを開いた時に呼ぶ
    /// </summary>
    public void PageStart(bool isFirst)
    {
        Debug.Log("is first = " + isFirst + " page index = " + pageIndex);
        
        if (isFirst)
        {
            //初回のみ行う設定
            ImportHoloObject();
            ImportTargetPoint();

            //絵本のメッシュに適用されているマテリアルを強制的に変更する
            Material visibleMat = MainSceneManager.I.visibleMat;
            for (int i = 0; i < bookObjects.Length; i++)
            {
                bookObjects[i].GetComponent<Renderer>().material = visibleMat;
            }
        }

        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].PageStart(pageIndex, isFirst);
        }
    }

    //ページに存在するActorタグのオブジェクトをDictionaryとListに格納する
    void ImportHoloObject()
    {
        GameObject[] tempArray;

        tempArray = GameObject.FindGameObjectsWithTag("Actor");
        for (int i = 0; i < tempArray.Length; i++)
        {
            HoloObject holoObject = tempArray[i].GetComponent<HoloObject>();
            if (holoObject == null)
            {
                Debug.LogWarning(tempArray[i].name + "is not actor");
                continue;
            }

            Debug.Log("add actor " + holoObject.name);
            objectList.Add(holoObject);

            try
            {
                objectDictionary.Add(holoObject.name, holoObject);
            }
            catch
            {
                //同名のKeyだと格納できない(Destroyとかの処理をする場合は固有の名前にしなければならない)
                Debug.LogWarning(holoObject.name + "was not import object dictionary");
                continue;
            }

            if (holoObject.GetActorType == HoloObject.HoloObjectType.Statics) continue;

            //動かせるオブジェクト
            movableObjectList.Add((HoloMovableObject)holoObject);

            try
            {
                movableObjectDictionary.Add(holoObject.name, (HoloMovableObject)holoObject);
            }
            catch
            {
                //動かすオブジェクトは必ず固有の名前にするべきなのでErrorを出す
                Debug.LogError(holoObject.name + "was not import movable dictionary");
                continue;
            }
        }
    }
    //ページに存在するTargetタグのオブジェクトをDictionaryに格納する
    void ImportTargetPoint()
    {
        GameObject[] tempArray;

        tempArray = GameObject.FindGameObjectsWithTag("Target");
        for (int i = 0; i < tempArray.Length; i++)
        {
            Debug.Log("add target point " + tempArray[i].name);
            try
            {
                targetPointDictionary.Add(tempArray[i].name, tempArray[i].transform);
            }
            catch
            {
                Debug.LogError(tempArray[i].name + "was not import target point ");
                continue;
            }
        }
    }

    public void PlayPage()
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].PlayPage();
        }
    }

    /// <summary>
    /// このページに登録されているアクターをページを開いた時の位置に戻す
    /// </summary>
    public void ResetPage()
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].ResetTransform();
        }
        //todo:リセット中のアニメーション
    }

    public void SetAllAgentEnabled(bool enabled)
    {
        for (int i = 0; i < movableObjectList.Count; i++)
        {
            movableObjectList[i].m_agent.enabled = enabled;
        }
    }

    public void AddMovableObject(HoloMovableObject obj)
    {
        movableObjectList.Add(obj);
        movableObjectDictionary.Add(obj.name, obj);
    }

    public void RemoveMovableObject(string name)
    {
        movableObjectList.Remove(movableObjectDictionary[name]);
        movableObjectDictionary.Remove(name);
    }
}
