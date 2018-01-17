using System.Collections.Generic;
using UnityEngine;
using KKUtilities;
    
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
    Don,
    Suzume1,
    Suzume2,
    Suzume3,
    Child1,
    Child2,
    Child3
}

/// <summary>
/// ホログラム全般を管理するクラス
/// </summary>
public class ActorManager : Singleton<ActorManager>
{
    public enum TargetType { StaticPoint, HoloObject }

    MainSceneManager sceneManager;
    BasePage currentPage;

    //動かせるオブジェクトの上に表示する三角のやつ
    public GameObject trianglePrefab = null;
    public HandIconController handIconControllerPrefab = null;

    //ページの外に置かれたオブジェクト
    Dictionary<string, HoloObject> globalObjectDictionary = new Dictionary<string, HoloObject>();
    
    public void InitSceneManager(MainSceneManager sceneManager)
    {
        this.sceneManager = sceneManager;

        sceneManager.OnPageLoaded += (page)=> OnPageLoaded(page);
        trianglePrefab = MyAssetStore.I.GetAsset<GameObject>("triangle", "Prefabs/");
        handIconControllerPrefab = MyAssetStore.I.GetAsset<GameObject>("HandIconController", "Prefabs/").GetComponent<HandIconController>();
    }

    /// <summary>
    /// 現在アクティブなアクターを返します
    /// </summary>
    public HoloCharacter GetCharacter(ActorName name)
    {
        HoloCharacter obj;
        if (currentPage.characterDictionary.TryGetValue(name, out obj))
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

    public Transform GetTargetTransform(string name, TargetType targetType)
    {
        Transform target = null;
        switch (targetType)
        {
            case TargetType.StaticPoint:
                target = GetTargetPoint(name);
                break;
            case TargetType.HoloObject:
                HoloObject obj = GetObject(name);
                if (obj == null) break;
                target = obj.transform;
                break;
        }

        return target;
    }

    /// <summary>
    /// 現在のページに登録されているアクターのアクティブを設定します
    /// </summary>
    public void SetEnableObject(string actorName, bool enabled)
    {
        HoloObject actor = GetObject(actorName);
        if (actor != null) actor.gameObject.SetActive(enabled);
        else Debug.LogError("didn't set enabled " + actorName);
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

    public void AddObject(HoloObject obj)
    {
        currentPage.objectDictionary.Add(obj.name, obj);
        currentPage.objectList.Add(obj);
    }

    public void AddCharacter(ActorName name, HoloCharacter character)
    {
        Debug.Log("add character " + currentPage.name + " " + name.ToString());
        currentPage.characterDictionary.Add(name, character);
        currentPage.objectDictionary.Add(character.name, character);
        currentPage.objectList.Add(character);
    }

    //ページが変更
    void OnPageLoaded(BasePage nextPage)
    {
        currentPage = nextPage;
    }
}
