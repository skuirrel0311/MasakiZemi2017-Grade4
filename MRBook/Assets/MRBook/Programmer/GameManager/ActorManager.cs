using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    protected override void Start()
    {
        base.Start();
        gameManager = MainSceneManager.I;
        gameManager.OnPageChanged += OnPageChanged;
    }

    /// <summary>
    /// 現在アクティブなアクターを返します
    /// </summary>
    public HoloObject GetActor(string name)
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

    //大量にコールすると重いかも
    public List<HoloObject> GetAllObject()
    {
        return new List<HoloObject>(currentPage.objectDictionary.Values).Where(n => n.GetActorType != HoloObject.HoloObjectType.Statics).ToList();
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
            previousPage.objectDictionary.Remove(key);
            globalObjectDictionary[key].transform.parent = nextPage.transform;
        }
        //アクティブなアクターはページを開いた時に追加されるのでここで追加はしない
    }
}
