using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

/// <summary>
/// ページを再生した結果を管理するクラス
/// </summary>
public class PageResultManager : BaseManager<PageResultManager>
{
    [SerializeField]
    HoloSprite sprite = null;
    [SerializeField]
    HoloText deatText = null;

    [SerializeField]
    Sprite[] sprites = null;
    
    //浦島は死んでしまったか？
    bool isDead = false;
    //ページを成功することができたか？
    bool isSuccess = false;

    public void SetResult(bool isSuccess)
    {
        this.isSuccess = isSuccess;
    }
    
    /// <summary>
    /// 死因をセットする
    /// </summary>
    public void SetDeadText(string causeOfDeath)
    {
        deatText.CurrentText = "死因：" + causeOfDeath;
        isDead = true;
    }

    public void ShowResult()
    {
        if (ResultManager.I.isGameOver) return;

        sprite.SetSprite(sprites[isSuccess ? 1 : 0]);

        deatText.gameObject.SetActive(true);
        sprite.gameObject.SetActive(true);

        Color tempColor;
        StartCoroutine(Utilities.FloatLerp(0.5f, (t) =>
        {
            tempColor = Color.Lerp(Color.clear, Color.white, t);
            deatText.Color = tempColor;
            sprite.Color = tempColor;
        }).OnCompleted(() =>
        {
            MainSceneManager.I.EndCallBack(isSuccess);
        }));

        if (isSuccess)
        {
            StartCoroutine(PlayHanabi());
        }
    }

    IEnumerator PlayHanabi()
    {
        Vector3 fireFlowerPosition;

        for (int i = 0; i < 5; i++)
        {
            fireFlowerPosition = transform.position;
            const float range = 0.5f;
            fireFlowerPosition.x += Random.Range(-range, range);
            fireFlowerPosition.y += Random.Range(-range, range);
            ParticleManager.I.Play("Hanabi", fireFlowerPosition);
            for (int j = 0; j < 10; j++) yield return null;
        }
    }

    public void Hide()
    {
        deatText.CurrentText = "";
        isDead = false;
        isSuccess = false;
        deatText.gameObject.SetActive(false);
        sprite.gameObject.SetActive(false);
    }
}
