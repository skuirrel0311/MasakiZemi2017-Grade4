using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class MyInputHandler : MonoBehaviour, IInputHandler, ISourceStateHandler
{
    public bool IsDragging { get; protected set; }

    protected IInputSource currentInputSource = null;
    protected uint currentInputSourceID;

    [SerializeField]
    protected GameObject targetObject = null;
    //オブジェクトを滑らかに動かすのに必要
    protected Interpolator interpolator;

    protected virtual void Start()
    {
        //ターゲットか自身からInterpolatorを取得する
        if (targetObject != null)
        {
            interpolator = targetObject.GetComponent<Interpolator>();
        }

        //なければ自身から
        if (interpolator == null)
        {
            interpolator = GetComponent<Interpolator>();
            targetObject = gameObject;
        }
    }
    protected virtual void Update()
    {
        if (!IsDragging) return;

        UpdateDragging();
    }

    protected virtual void StartDragging()
    {
        //入力イベントの対象を固定
        InputManager.Instance.PushModalInputHandler(gameObject);

        IsDragging = true;
    }
    protected virtual void UpdateDragging() { }
    protected virtual void StopDragging()
    {
        IsDragging = false;
        //固定の解除
        InputManager.Instance.PopModalInputHandler();
        currentInputSource = null;
    }

    //指が倒された
    public void OnInputDown(InputEventData eventData)
    {
        if (IsDragging) return;

        //手の位置が検出できない場合は終了
        if (!eventData.InputSource.SupportsInputInfo(eventData.SourceId, SupportedInputInfo.Position)) return;

        currentInputSource = eventData.InputSource;
        currentInputSourceID = eventData.SourceId;

        StartDragging();
    }

    //指が持ち上げられた
    public void OnInputUp(InputEventData eventData)
    {
        //フィルター
        if (currentInputSource == null) return;
        if (eventData.SourceId != currentInputSourceID) return;
        if (!IsDragging) return;

        StopDragging();
    }

    //手を見つけた
    public void OnSourceDetected(SourceStateEventData eventData) { }

    //手を見失った
    public void OnSourceLost(SourceStateEventData eventData)
    {
        if (!IsDragging) return;
        if (currentInputSource == null) return;
        if (eventData.SourceId != currentInputSourceID) return;

        StopDragging();
    }
}
