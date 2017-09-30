using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum ActorName
{
    Urashima,
    Turtle,
    Otohime,
    Akane,
    Ai,
    Midori,
    Lemon,
    Gen,
    Ryu,
    Don
}

/// <summary>
/// ホログラム全般を管理するクラス
/// </summary>
public class ActorManager : BaseManager<ActorManager>
{
    MainSceneManager gameManager;
    [SerializeField]
    BasePage currentPage;
    
    //ページの外に置かれたオブジェクト
    Dictionary<string, HoloObject> globalObjectDictionary = new Dictionary<string, HoloObject>();

    string[] actorNameList;
    string[] ActorNameList
    {
        get
        {
            if (actorNameList != null) return actorNameList;

            int num = Enum.GetNames(typeof(ActorName)).Length;
            for(int i = 0;i < num;i++)
            {
            }

            return actorNameList;
        }
    }

    protected override void Start()
    {
        base.Start();
        gameManager = MainSceneManager.I;
        gameManager.OnPageChanged += OnPageChanged;
    }

    /// <summary>
    /// 現在アクティブなアクターを返します
    /// </summary>
    public HoloMovableObject GetActor(string name)
    {
        HoloMovableObject obj;
        if (currentPage.movableObjectDictionary.TryGetValue(name, out obj))
        {
            return obj;
        }

        Debug.LogError("not found " + name);
        return null;
    }

    public HoloObject GetObject(string name)
    {
        HoloObject obj;
        if (currentPage.objectDictionary.TryGetValue(name, out obj))
        {
            return obj;
        }

        Debug.LogError("not found " + name);
        return null;
    }

    /// <summary>
    /// 指定された名前のターゲットポイントを返します
    /// </summary>
    public Transform GetTargetPoint(string name)
    {
        Transform t;
        if (currentPage.targetPointDictionary.TryGetValue(name, out t))
        {
            return t;
        }

        Debug.LogError("not found target point " + name);
        return null;
    }

    /// <summary>
    /// 現在のページに登録されているアクターを非表示にします
    /// </summary>
    public void DisableActor(string actorName)
    {
        //todo:消す時のエフェクト？
        HoloObject actor = GetActor(actorName);
        if (actor != null) actor.gameObject.SetActive(false);
        else Debug.LogError("didn't disable " + actorName);
    }

    /// <summary>
    /// グローバルオブジェクトに追加します
    /// </summary>
    public void SetGlobal(HoloObject actor)
    {
        if (globalObjectDictionary.ContainsKey(actor.name))
        {
            Debug.LogError(actor.name + "is already added");
            return;
        }
        globalObjectDictionary.Add(actor.name, actor);
    }

    /// <summary>
    /// グローバルオブジェクトから削除します
    /// </summary>
    /// <param name="actor"></param>
    public void RemoveGlobal(string name)
    {
        globalObjectDictionary.Remove(name);
    }
    
    public List<HoloObject> GetAllObject()
    {
        return currentPage.objectList;
    }

    public List<HoloMovableObject> GetAllActor()
    {
        return currentPage.movableObjectList;
    }

    //ページが変更
    void OnPageChanged(BasePage previousPage, BasePage nextPage)
    {
        currentPage = nextPage;
        
        if (globalObjectDictionary.Count == 0) return;
        //前のページから持ってきたオブジェクトがある。
        
        foreach(string key in globalObjectDictionary.Keys)
        {
            //前のページから登録を消す
            previousPage.movableObjectDictionary.Remove(key);
            globalObjectDictionary[key].transform.parent = nextPage.transform;
        }
        //アクティブなアクターはページを開いた時に追加されるのでここで追加はしない
    }
}
