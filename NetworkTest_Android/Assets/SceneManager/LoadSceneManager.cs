using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneManager : BaseManager<LoadSceneManager>
{
    Image panel = null;

    Coroutine fadeCoroutine = null;

    protected override void Start()
    {
        base.Start();
        panel = transform.FindChild("Panel").GetComponent<Image>();
    }

    public void LoadScene(string loadSceneName, bool isAsync = false)
    {
        try
        {
            if (isAsync)
                SceneManager.LoadSceneAsync(loadSceneName);
            else
                SceneManager.LoadScene(loadSceneName);
        }
        catch
        {
            Debug.LogError("LoadSceneNotFound");
        }
    }

    public void LoadScene(string loadSceneName, bool isAsync = false, float duration = 1.0f, float wait = 1.0f)
    {
        FadeOut(duration, () =>
        {
            LoadScene(loadSceneName, true);
            FadeIn(duration, wait);
        });
    }

    IEnumerator FadeCoroutine(Color startColor, Color endColor, float duration, Action action, float wait)
    {
        if (panel == null)
        {
            if (action != null) action.Invoke();
            yield break;
        }

        panel.gameObject.SetActive(true);
        if (wait != 0.0f)
        {
            yield return new WaitForSeconds(wait);
        }

        //現在のパネルの色とフェードを開始する色に差異があったら0.5秒で調整する
        if (panel.color != startColor)
        {
            Color temp = panel.color;
            yield return
                StartCoroutine(KKUtilities.FloatLerp(0.5f, (t) =>
                {
                    panel.color = Color.Lerp(temp, startColor, t);
                }));
        }

        yield return
            StartCoroutine(KKUtilities.FloatLerp(duration, (t) =>
            {
                panel.color = Color.Lerp(startColor, endColor, t);
            }));

        fadeCoroutine = null;
        panel.gameObject.SetActive(false);
        if (action != null) action.Invoke();
    }

    void FadeIn(float duration, float wait)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeCoroutine(Color.black, Color.clear, duration, null, wait));
    }

    void FadeOut(float duration, Action action = null)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeCoroutine(Color.clear, Color.black, duration, action, 0.0f));
    }
}
