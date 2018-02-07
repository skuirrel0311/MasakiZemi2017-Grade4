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
    Child3,
    NiceGuy
}

/// <summary>
/// ホログラム全般を管理するクラス
/// </summary>
public class ActorManager : Singleton<ActorManager>
{
    public enum TargetType { StaticPoint, HoloObject }

    MainSceneManager sceneManager;

    //動かせるオブジェクトの上に表示する三角のやつ
    public GameObject trianglePrefab = null;
    public HandIconController handIconControllerPrefab = null;

    //ページの外に置かれたオブジェクト
    Dictionary<string, HoloObject> globalObjectDictionary = new Dictionary<string, HoloObject>();
    
    public void InitSceneManager(MainSceneManager sceneManager)
    {
        this.sceneManager = sceneManager;
        
        trianglePrefab = MyAssetStore.I.GetAsset<GameObject>("triangle", "Prefabs/");
        handIconControllerPrefab = MyAssetStore.I.GetAsset<GameObject>("HandIconController", "Prefabs/").GetComponent<HandIconController>();
    }

    /// <summary>
    /// 現在アクティブなアクターを返します
    /// </summary>
    public HoloCharacter GetCharacter(ActorName name)
    {
        HoloCharacter obj;
        if (sceneManager.pages[sceneManager.currentPageIndex].characterDictionary.TryGetValue(name, out obj))
        {
            return obj;
        }

        Debug.LogError("not found " + name);
        return null;
    }

    public HoloObject GetObject(string name)
    {
        HoloObject obj;
        if (sceneManager.pages[sceneManager.currentPageIndex].objectDictionary.TryGetValue(name, out obj))
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
        if (sceneManager.pages[sceneManager.currentPageIndex].targetPointDictionary.TryGetValue(name, out t))
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
        return sceneManager.pages[sceneManager.currentPageIndex].objectList;
    }

    public void AddObject(HoloObject obj, bool addResetter = true)
    {
        sceneManager.pages[sceneManager.currentPageIndex].objectDictionary.Add(obj.name, obj);
        sceneManager.pages[sceneManager.currentPageIndex].objectList.Add(obj);

        obj.Init();
        if(addResetter) HoloObjResetManager.I.AddResetter(obj.Resetter);
    }

    public void AddCharacter(ActorName name, HoloCharacter character, bool addResetter = true)
    {
        Debug.Log("add character " + sceneManager.pages[sceneManager.currentPageIndex].name + " " + name.ToString());
        sceneManager.pages[sceneManager.currentPageIndex].characterDictionary.Add(name, character);

        AddObject(character, addResetter);
    }
}
