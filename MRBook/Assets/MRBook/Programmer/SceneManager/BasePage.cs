using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePage : MonoBehaviour
{
    /// <summary>
    /// お題
    /// </summary>
    public string missionText = "";

    [Multiline]
    public string storyFirstText = "";

    [Multiline]
    public string storyEndText = "";

    public int lifePoint = 3;

    [SerializeField]
    int pageIndex = 0;
    public int Index { get { return pageIndex; } }

    bool isFirst = true;

    public RuntimeAnimatorController controller = null;

    [SerializeField]
    HoloObject[] defaultDisableObjects = null;

    /// <summary>
    /// オクルージョン用のオブジェクト
    /// </summary>
    [SerializeField]
    GameObject[] bookObjects = null;

    public List<GameObject> rightSideObjectList = new List<GameObject>();
    public List<GameObject> leftSideObjectList = new List<GameObject>();
    public List<GameObject> middleSideObjectList = new List<GameObject>();

    [SerializeField]
    float leftSideHeightOffset = 0.0f;
    [SerializeField]
    float rightSideHeightOffset = 0.0f;
    [SerializeField]
    float middleSideHeightOffset = 0.0f;

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
    }

    public void SetTransform(Transform t)
    {
        if (MainSceneManager.I.currentPageIndex == pageIndex)
            StartCoroutine(SetPosition(t));
        else
            transform.SetPositionAndRotation(t.position, t.rotation);
    }

    //フレームをまたがないとAgentの有効無効がきちんと反映されなかったので
    IEnumerator SetPosition(Transform t)
    {
        SetAllObjectActive(false);
        yield return null;

        transform.SetPositionAndRotation(t.position, t.rotation);

        yield return null;

        MyNavMeshBuilder.CreateNavMesh();

        yield return null;

        SetAllObjectActive(true);

    }

    /// <summary>
    /// ページを開いた時に呼ぶ
    /// </summary>
    public virtual void PageStart()
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
                Material[] materials = bookObjects[i].GetComponent<Renderer>().materials;
                for (int j = 0; j < materials.Length; j++)
                {
                    materials[j] = visibleMat;
                }

                bookObjects[i].GetComponent<Renderer>().materials = materials;
            }
        }

        Vector3 upVec = Vector3.up;

        float rightOffset = rightSideHeightOffset * MainSceneManager.I.bookOffsetCoefficient;
        float leftOffset = leftSideHeightOffset * MainSceneManager.I.bookOffsetCoefficient;
        float middleOffset = middleSideHeightOffset * MainSceneManager.I.bookOffsetCoefficient;

        for (int i = 0;i< rightSideObjectList.Count;i++)
        {
            rightSideObjectList[i].transform.localPosition += (upVec * rightOffset);
        }

        for(int i = 0;i< leftSideObjectList.Count;i++)
        {
            leftSideObjectList[i].transform.localPosition += (upVec * leftOffset);
        }

        for (int i = 0; i < middleSideObjectList.Count; i++)
        {
            middleSideObjectList[i].transform.localPosition += (upVec * middleOffset);
        }

        HoloObjResetManager resetManager = HoloObjResetManager.I;
        for (int i = 0; i < objectList.Count; i++)
        {
            resetManager.AddResetter(objectList[i].Resetter);
            objectList[i].PageStart(pageIndex);
        }
        
        isFirst = false;
    }

    //ページに存在するActorタグのオブジェクトをDictionaryとListに格納する
    void ImportHoloObject()
    {
        //非アクティブなオブジェクトを追加していく

        for(int i = 0;i< defaultDisableObjects.Length;i++)
        {
            HoloObject obj = defaultDisableObjects[i];
            
            objectList.Add(obj);
            if(obj.useHeightOffset) IntoSideList(obj.gameObject);

            objectDictionary.Add(obj.name, obj);
            if (obj.GetActorType != HoloObject.Type.Character) continue;
            characterDictionary.Add((ActorName)Enum.Parse(typeof(ActorName), obj.name), (HoloCharacter)obj);
        }

        //アクティブなオブジェクトを追加していく
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
            if (holoObject.useHeightOffset) IntoSideList(holoObject.gameObject);

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
                IntoSideList(tempArray[i].gameObject);
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
    public virtual void ResetPage()
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

    void IntoSideList(GameObject obj)
    {
        float cross = GetCrossValue(transform.forward, Vector3.Normalize(obj.transform.position - transform.position));
        float length = Vector3.Magnitude(obj.transform.position - transform.position);
        
        //Debug.Log(obj.name +  " cross = " + cross + " length = " + length);

        if (Mathf.Abs(length) > 0.2f && Mathf.Abs(cross) >= 0.01)
        {
            if(cross > 0)
            {
                //左側
                leftSideObjectList.Add(obj);
            }
            else
            {
                //右側
                rightSideObjectList.Add(obj);
            }
        }
        else
        {
            //真ん中
            middleSideObjectList.Add(obj);
        }

    } 
    
    //正規化したベクトルを渡す
    float GetDotValue(Vector3 vec1, Vector3 vec2)
    {
        return (vec1.x * vec2.x) + (vec1.z * vec2.z);
    }

    float GetCrossValue(Vector3 vec1, Vector3 vec2)
    {
        return (vec1.x * vec2.z) - (vec2.x * vec1.z);
    }
}
