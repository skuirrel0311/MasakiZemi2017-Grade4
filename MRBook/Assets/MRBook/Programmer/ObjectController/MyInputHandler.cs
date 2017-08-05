using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

public class MyInputHandler : MonoBehaviour, IInputHandler, ISourceStateHandler
{
    protected bool isDragging = false;

    protected IInputSource currentInputSource = null;
    protected uint currentInputSourceID;

    //頭の位置
    [SerializeField]
    protected Transform mainCameraTransform;

    [SerializeField]
    protected GameObject targetObject = null;
    //オブジェクトを滑らかに動かすのに必要
    protected Interpolator interpolator;

    protected virtual void Start()
    {
        if(mainCameraTransform == null)
            mainCameraTransform = Camera.main.transform;

        if(mainCameraTransform == null)
            mainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;

        //ターゲットか自身からInterpolatorを取得する
        if(targetObject != null)
        {
            interpolator = targetObject.GetComponent<Interpolator>();
        }

        if(interpolator == null)
        {
            interpolator = GetComponent<Interpolator>();
            targetObject = gameObject;
        }
    }
    protected virtual void Update()
    {
        if (!isDragging) return;

        UpdateDragging();
    }

    protected virtual void StartDragging()
    {
        //入力イベントの対象を固定
        InputManager.Instance.PushModalInputHandler(gameObject);

        isDragging = true;
    }
    protected virtual void UpdateDragging() { }
    protected virtual void StopDragging()
    {
        isDragging = false;
        //固定の解除
        InputManager.Instance.PopModalInputHandler();
        currentInputSource = null;
    }

    protected Vector3 GetHandPosition()
    {
        Vector3 handPosition;
        currentInputSource.TryGetPosition(currentInputSourceID, out handPosition);
        return handPosition;
    }

    //指が倒された
    public void OnInputDown(InputEventData eventData)
    {
        if (isDragging) return;

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
        if (!isDragging) return;

        StopDragging();
    }

    //手を見つけた
    public void OnSourceDetected(SourceStateEventData eventData) { }

    //手を見失った
    public void OnSourceLost(SourceStateEventData eventData)
    {
        //フィルター
        if (currentInputSource == null) return;
        if (eventData.SourceId != currentInputSourceID) return;
        if (!isDragging) return;
        
        StopDragging();
    }
}
