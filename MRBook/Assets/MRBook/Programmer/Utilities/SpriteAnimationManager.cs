using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// １行で呼び出しができて、オブジェクトプールができていることを目指す
/// </summary>
public class SpriteAnimationManager : BaseManager<SpriteAnimationManager>
{
    Dictionary<string, SpriteAnimation> spriteAnimationDictionary = new Dictionary<string, SpriteAnimation>();

    public void Play(string name, Vector3 position, float lifeTime)
    {
        SpriteAnimation spriteAnimation;

        spriteAnimation = GetLastSprite(name);

        spriteAnimation.transform.position = position;
        spriteAnimation.Play(lifeTime);
    }

    SpriteAnimation GetLastSprite(string name)
    {
        SpriteAnimation lastSprite = null;
        string currentName = name;

        for (int i = 0; ; i++)
        {
            //連番が既に格納されているかチェックする
            if(i != 0) currentName = name + i;
            if (spriteAnimationDictionary.TryGetValue(currentName, out lastSprite))
            {
                if (!lastSprite.IsPlaying)
                    break;
                else //連番が再生中だったので次を探しに行く
                    continue;
            }
            else
            {
                //連番が存在しなかった(生成する必要がある)
                SpriteAnimation spritePrefab = MyAssetStore.I.GetAsset<SpriteAnimation>(name, "SpriteAnimations/");
                lastSprite = Instantiate(spritePrefab, transform);
                lastSprite.Init();
                spriteAnimationDictionary.Add(currentName, lastSprite);

                break;
            }
        }

        return lastSprite;
    }
}
