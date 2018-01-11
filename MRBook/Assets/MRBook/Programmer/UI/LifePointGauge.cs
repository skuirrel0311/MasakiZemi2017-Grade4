using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePointGauge : BaseGauge
{
    SpriteRenderer[] lifeSprites;
    [SerializeField]
    SpriteRenderer lifeSpritePrefab = null;
    
    //spriteを並べるときにどれだけ離すか
    [SerializeField]
    Vector3 spriteDistance = Vector3.zero;

    public void Init(int maxValue)
    {
        base.Init();

        this.maxValue = maxValue;

        lifeSprites = new SpriteRenderer[maxValue];

        SpriteRenderer sr;
        for (int i = 0; i < maxValue; i++)
        {
            sr = Instantiate(lifeSpritePrefab, transform);
            sr.transform.position = transform.position + (spriteDistance * i);

            lifeSprites[i] = sr;            
        }
    }

    protected override void UpdateGaugeImage()
    {
        for(int i = 0;i< maxValue;i++)
        {
            lifeSprites[i].enabled = i < value;
        }
    }
}
