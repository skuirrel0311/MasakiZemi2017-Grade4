using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class Fader : BaseManager<Fader>
{
    [SerializeField]
    Shader fadeShader = null;

    ActorManager actorManager;

    Coroutine fadeCoroutine;
    Action callback;

    BookPositionModifier positionModifier;

    public enum State { Wait, FadeIn, FadeOut }
    public State CurrentState;
    //public State CurrentState { get; private set; }

    protected override void Start()
    {
        base.Start();

        actorManager = ActorManager.I;
        positionModifier = BookPositionModifier.I;
        CurrentState = State.Wait;
    }

    public void FadeIn(Action callback = null)
    {
        this.callback = callback;
        CurrentState = State.FadeIn;
        fadeCoroutine = StartCoroutine(Fade(positionModifier.bookTransform.position.y + 0.5f,positionModifier.bookTransform.position.y +  0.0f));
    }

    public void FadeOut(Action callback = null)
    {
        this.callback = callback;
        CurrentState = State.FadeOut;
        List<HoloObject> objList = actorManager.GetAllObject();

        //オブジェクトを見えないようにしておく
        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].SetFadeShader(fadeShader);
            if (objList[i].m_materials == null) continue;
            for (int j = 0; j < objList[i].m_materials.Length; j++)
            {
                objList[i].m_materials[j].SetFloat("_Height", 0.0f);
            }
        }

        //FadeOut開始
        fadeCoroutine = StartCoroutine(Fade(positionModifier.bookTransform.position.y + 0.0f, positionModifier.bookTransform.position.y + 0.5f, 5.0f, 0.5f));
    }

    IEnumerator Fade(float start, float end,  float duration = 2.0f, float startDelay = 0.0f)
    {
        if(startDelay > 0)
        {
            yield return new WaitForSeconds(startDelay);
        }

        List<HoloObject> objList = actorManager.GetAllObject();

        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].SetFadeShader(fadeShader);
        }

        yield return StartCoroutine(Utilities.FloatLerp(duration, (t) =>
        {
            float temp = Mathf.Lerp(start, end, t);

            for (int i = 0; i < objList.Count; i++)
            {
                if (objList[i].m_materials == null) continue;
                for (int j = 0; j < objList[i].m_materials.Length; j++)
                {
                    objList[i].m_materials[j].SetFloat("_Height", temp);
                }
            }
        }));

        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].ResetShader();
        }

        //コールバック
        CurrentState = State.Wait;
        if (callback != null) callback.Invoke();
    }

    public void AddCallBack(Action action)
    {
        callback += action;
    }
}
