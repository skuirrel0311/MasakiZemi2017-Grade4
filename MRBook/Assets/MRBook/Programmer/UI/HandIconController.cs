using UnityEngine;

//渡されたCharacterItemSaucerに合わせて手のアイコンを調整する
public class HandIconController : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites = null;
    CharacterItemSaucer itemSaucer;

    [SerializeField]
    HoloButton rightButton = null;
    [SerializeField]
    HoloButton leftButton = null;

    public void Init(CharacterItemSaucer itemSaucer)
    {
        this.itemSaucer = itemSaucer;
        MyObjControllerByBoundingBox.I.OnItemDragStart += Show;
        MyObjControllerByBoundingBox.I.OnItemDragEnd += Hide;
    }

    public void Show()
    {
        rightButton.gameObject.SetActive(true);
        leftButton.gameObject.SetActive(true);

        Sprite rightSprite = sprites[itemSaucer.HasItem_Right ? 2 : 0];
        Sprite leftSprite = sprites[itemSaucer.HasItem_Left ? 3 : 1];

        rightButton.spriteRenderer.sprite = rightSprite;
        leftButton.spriteRenderer.sprite = leftSprite;
    }

    public void Hide()
    {
        rightButton.gameObject.SetActive(false);
        leftButton.gameObject.SetActive(false);
    }
}
