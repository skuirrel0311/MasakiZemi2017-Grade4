using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class BasePage : MonoBehaviour
{
    /// <summary>
    /// お題
    /// </summary>
    public string missionText = "";

    [SerializeField]
    int pageIndex = 0;
    public int Index { get { return pageIndex; } }

    bool isFirst = true;

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
    //モーションするキャラクター
    public Dictionary<ActorName, HoloCharacter> characterDictionary = new Dictionary<ActorName, HoloCharacter>();

    //ページに存在するホログラム全部。
    public List<HoloObject> objectList = new List<HoloObject>();


    /// <summary>
    /// ゲーム開始時に本の位置を固定させるやつ(キャラクタなどが移動していないことが保証されている)
    /// </summary>
    public void PageLock(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);

        //動く可能性のあるやつの開始時の位置を保存
        //for (int i = 0; i < movableObjectList.Count; i++)
        //{
        //    movableObjectList[i].ApplyDefaultTransform();
        //}
    }

    public void SetTransform(Vector3 position, Quaternion rotation)
    {
        if (MainSceneManager.I.currentPageIndex == pageIndex)
            StartCoroutine(SetPosition(position, rotation));
        else
            transform.SetPositionAndRotation(position, rotation);
    }

    //フレームをまたがないとAgentの有効無効がきちんと反映されなかったので
    IEnumerator SetPosition(Vector3 position, Quaternion rotation)
    {
        SetAllObjectActive(false);
        yield return StartCoroutine(Utilities.Delay(5, () =>
        {
            transform.SetPositionAndRotation(position, rotation);
        }));
        
        yield return StartCoroutine(Utilities.Delay(5, () =>
        {
            SetAllObjectActive(true);
        }));

    }

    /// <summary>
    /// ページを開いた時に呼ぶ
    /// </summary>
    public void PageStart()
    {
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
            objectList[i].PageStart(pageIndex);
        }

        isFirst = false;
    }

    //ページに存在するActorタグのオブジェクトをDictionaryとListに格納する
    void ImportHoloObject()
    {
        HoloObjResetManager resetManager = HoloObjResetManager.I;
        GameObject[] tempArray;

        tempArray = GameObject.FindGameObjectsWithTag("Actor");
        for (int i = 0; i < tempArray.Length; i++)
        {
            HoloObject holoObject = tempArray[i].GetComponent<HoloObject>();
            if (holoObject == null)
            {
                //ActorタグなのにHoloObjectが取得できないのはやばい
                Debug.LogWarning(tempArray[i].name + "is not actor");
                continue;
            }

            //Debug.Log("add actor " + holoObject.name);
            objectList.Add(holoObject);
            resetManager.AddResetter(holoObject.Resetter);

            try
            {
                objectDictionary.Add(holoObject.name, holoObject);
            }
            catch
            {
                //同名のKeyだと格納できない(Destroyとかの処理をする場合は固有の名前にしなければならない)
                Debug.Log(holoObject.name + "was not import object dictionary");
                continue;
            }

            if (holoObject.GetActorType != HoloObject.Type.Character) continue;

            try
            {
                characterDictionary.Add((ActorName)Enum.Parse(typeof(ActorName), holoObject.name), (HoloCharacter)holoObject);
            }
            catch
            {
                //キャラクターは必ず固有の名前にするべきなのでErrorを出す
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
            //Debug.Log("add target point " + tempArray[i].name);
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
        HoloObjResetManager.I.Reset();
        //todo:リセット中のアニメーション
    }

    public void SetAllObjectActive(bool active)
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].gameObject.SetActive(active);
        }
    }

    public void RemoveMovableObject(string name)
    {
        //HoloObject obj;
        //if (!objectDictionary.TryGetValue(name, out obj)) return;

        //movableObjectList.Remove((HoloMovableObject)obj);

        //if (!((HoloMovableObject)obj).IsGrounding) return;

        //groundingObjectList.Remove((HoloGroundingObject)obj);

        //if (obj.GetActorType != HoloObject.Type.Character) return;

        //characterDictionary.Remove((ActorName)Enum.Parse(typeof(ActorName), obj.name));
    }
}
