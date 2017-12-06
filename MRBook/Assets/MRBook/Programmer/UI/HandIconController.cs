using UnityEngine;

//渡されたCharacterItemSaucerに合わせて手のアイコンを調整する
public class HandIconController : BaseManager<HandIconController>
{
    [SerializeField]
    Sprite[] sprites = null;
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

        Vector3 iconPosition = itemSaucer.Owner.transform.position;
        iconPosition.y += 1.2f * itemSaucer.Owner.transform.lossyScale.y;
        transform.position = iconPosition;

        float scale = 1.0f / transform.lossyScale.x;
        transform.localScale = Vector3.one * scale;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(leftButton.gameObject.activeSelf) leftButton.Push();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(rightButton.gameObject.activeSelf) rightButton.Push();
        }

        //todo:常にプレイヤーの方に向ける(billboard)
    }

    public void Show()
    {
        IsVisuable = true;

        rightButton.gameObject.SetActive(true);
        leftButton.gameObject.SetActive(true);
        
        rightButton.AddListener(() => itemSaucer.DumpItem(itemSaucer.RightHandItem));
        leftButton.AddListener(() => itemSaucer.DumpItem(itemSaucer.LeftHandItem));

        Sprite rightSprite = sprites[itemSaucer.HasItem_Right ? 2 : 0];
        Sprite leftSprite = sprites[itemSaucer.HasItem_Left ? 3 : 1];

        rightButton.spriteRenderer.sprite = rightSprite;
        leftButton.spriteRenderer.sprite = leftSprite;
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
