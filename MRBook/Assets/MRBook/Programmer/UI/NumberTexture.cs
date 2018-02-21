using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberTexture : MonoBehaviour
{
    //１０枚ある前提
    [SerializeField]
    Sprite[] numberTextures = null;

    [SerializeField]
    SpriteRenderer spritePrefab = null;

    [SerializeField]
    Vector3 spriteDistance = Vector3.zero;

    //1の位から順に格納していく
    public List<Sprite> GetNumberTextures(int number)
    {
        if (number < 0) return null;

        List<Sprite> spriteList = new List<Sprite>();

        int digit = 10;

        while (true)
        {
            int temp = number % digit;
            int digitValue = temp / (int)(digit * 0.1f);
            spriteList.Add(numberTextures[digitValue]);

            number -= temp;
            if (number <= 0) break;
            //次の位を調べる
            digit *= 10;
        }

        return spriteList;
    }

    public void SetNumber(int number)
    {
        List<Sprite> spriteList = GetNumberTextures(number);

        if (spriteList == null || spriteList.Count == 0) return;

        SpriteRenderer sr;
        for (int i = 0; i < spriteList.Count; i++)
        {
            sr = Instantiate(spritePrefab, transform);
            sr.transform.position = transform.position + (spriteDistance * i);
            sr.sprite = spriteList[i];
        }
    }
}
