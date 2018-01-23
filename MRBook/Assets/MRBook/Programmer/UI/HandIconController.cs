using UnityEngine;

//渡されたCharacterItemSaucerに合わせて手のアイコンを調整する
public class HandIconController : BaseManager<HandIconController>
{
    CharacterItemSaucer itemSaucer;

    [SerializeField]
    HoloButton button = null;

    public bool IsVisuable { get; private set; }

    protected override void Start()
    {
        base.Start();

        MainSceneManager.I.OnPlayPage += () =>
        {
            Hide();
        };
    }

    public void Init(CharacterItemSaucer itemSaucer)
    {
        this.itemSaucer = itemSaucer;

        //キャラクターの頭上に出す
        Vector3 iconPosition = itemSaucer.Owner.transform.position;
        iconPosition.y += 1.2f * itemSaucer.Owner.transform.lossyScale.y;
        transform.position = iconPosition;

        //キャラクタの大きさは変わらないはずなのでいらないかも
        float scale = 1.0f / transform.lossyScale.x;
        transform.localScale = Vector3.one * scale;
    }

    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(button.gameObject.activeSelf) button.Push();
        }
#endif
    }

    public void Show()
    {
        if (!MyObjControllerByBoundingBox.I.canClick) return;
        IsVisuable = true;

        button.gameObject.SetActive(true);
        
        button.AddListener(() => itemSaucer.DumpItem());
        HoloButton.ButtonState rightButtonState = itemSaucer.HasItem_Right || itemSaucer.HasItem_Left ? HoloButton.ButtonState.Normal : HoloButton.ButtonState.Pressed;
        button.SetDefaultButtonState(rightButtonState);
    }

    public void Hide()
    {
        IsVisuable = false;

        button.gameObject.SetActive(false);

        button.RemoveAllListener();
    }
}
