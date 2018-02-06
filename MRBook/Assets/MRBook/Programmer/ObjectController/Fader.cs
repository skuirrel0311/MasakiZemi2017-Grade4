using System;
using System.Collections;
using UnityEngine;
using KKUtilities;

public class Fader : BaseManager<Fader>
{
    [SerializeField]
    GameObject cube = null;

    Transform m_transform;

    public void FadeIn(Action callback = null)
    {
        cube.SetActive(true);
        StartCoroutine(Fade(0.0f, 1.0f, ()=>
        {
            if (callback != null) callback.Invoke();
        }));
    }

    public void FadeOut(Action callback = null)
    {
        StartCoroutine(Fade(1.0f, 0.0f, ()=>
        {
            if(callback != null) callback.Invoke();
            cube.SetActive(false);
        }));
    }

    IEnumerator Fade(float start, float end,Action callback = null, float duration = 2.0f)
    {
        Vector3 startScale = new Vector3(1.0f, start, 1.0f);
        Vector3 endScale = new Vector3(1.0f, end, 1.0f);
        cube.SetActive(true);

        yield return StartCoroutine(Utilities.FloatLerp(duration, (t) =>
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
        }));

        if (callback != null) callback.Invoke();
    }
}
