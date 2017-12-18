using UnityEngine;

//渡されたCharacterItemSaucerに合わせて手のアイコンを調整する
public class HandIconController : BaseManager<HandIconController>
{
    CharacterItemSaucer itemSaucer;

    [SerializeField]
    HoloButton rightButton = null;
    [SerializeField]
    HoloButton leftButton = null;

    public bool IsVisuable { get; private set; }

    public void Init(CharacterItemSaucer itemSaucer)
    {
        if (itemSaucer.Equals(this.itemSaucer)) return;

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

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(leftButton.gameObject.activeSelf) leftButton.Push();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(rightButton.gameObject.activeSelf) rightButton.Push();
        }
#endif
    }

    public void Show()
    {
        IsVisuable = true;

        rightButton.gameObject.SetActive(true);
        leftButton.gameObject.SetActive(true);
        
        rightButton.AddListener(() => itemSaucer.DumpItem(itemSaucer.RightHandItem));
        leftButton.AddListener(() => itemSaucer.DumpItem(itemSaucer.LeftHandItem));
        HoloButton.ButtonState rightButtonState = itemSaucer.HasItem_Right ? HoloButton.ButtonState.Normal : HoloButton.ButtonState.Pressed;
        HoloButton.ButtonState leftButtonState = itemSaucer.HasItem_Left ? HoloButton.ButtonState.Normal : HoloButton.ButtonState.Pressed;
        rightButton.SetDefaultButtonState(rightButtonState);
        leftButton.SetDefaultButtonState(leftButtonState);
    }

    public void Hide()
    {
        IsVisuable = false;

        rightButton.gameObject.SetActive(false);
        leftButton.gameObject.SetActive(false);

        rightButton.RemoveAllListener();
        leftButton.RemoveAllListener();
    }
}
