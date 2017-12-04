using UnityEngine;

//渡されたCharacterItemSaucerに合わせて手のアイコンを調整する
public class HandIconController : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites = null;
    CharacterItemSaucer itemSaucer;

    HoloButton rightButton;
    HoloButton leftButton;

    public void Init(CharacterItemSaucer itemSaucer)
    {
        this.itemSaucer = itemSaucer;
    }

    public void Show()
    {
        
    }

    public void Hide()
    {

    }
}
