using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtBookDoor : ArtBookGimmick
{
    [SerializeField]
    GameObject arrowSprite = null;

    [SerializeField]
    Transform openPositionTransform = null;

    [SerializeField]
    Animator eyeAnimator = null;

    float maxCrossValue;

    Vector3 cameraPosition;

    //どれだけ開いているか？
    float amount = 0.0f;
    float oldAmount = 0.0f;

    protected override void Start()
    {
        cameraPosition = transform.position + transform.up;

        Vector3 vec1 = (original.transform.position - cameraPosition).normalized;
        Vector3 vec2 = (openPositionTransform.position - cameraPosition).normalized;
        maxCrossValue = GetCrossValue(vec1, vec2);

        base.Start();
    }

    protected override void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.D))
        {
            isHide = !isHide;
            SetGimmickVisuable(!isHide);
            if (!isHide) AkSoundEngine.PostEvent("Eye", gameObject);
            else AkSoundEngine.PostEvent("Door", gameObject);
            NotificationManager.I.ShowMessage("ドアが" + (isHide ? "開いた" : "閉じた"), 1.0f);
        }

#else
        //初回だけ弾く
        if (!maker.IsVisuabled) return;

        if (arrowSprite.activeSelf != maker.IsVisuable)
        {
            eyeAnimator.SetBool("IsFound", maker.IsVisuable);
            if (maker.IsVisuable)
            {
                //見つけた
                AkSoundEngine.PostEvent("Eye", gameObject);
            }
            else
            {
                if(amount > 0.3f)
                {
                    //開いた
                    AkSoundEngine.PostEvent("Door", gameObject);
                }
                else
                {
                    //開いていない可能性がある
                    return;
                }
            }
            arrowSprite.SetActive(maker.IsVisuable);
        }

        if(maker.IsVisuable)
        {
            cameraPosition = transform.position + transform.up;
            Vector3 vec1 = (original.transform.position - cameraPosition).normalized;
            Vector3 vec2 = (maker.child.position - cameraPosition).normalized;
            float crossValue = GetCrossValue(vec1, vec2);
            float t = crossValue / maxCrossValue;
            amount = t;
            transform.position = Vector3.Lerp(original.transform.position, openPositionTransform.position, t);
        }
#endif

    }

    float GetCrossValue(Vector3 vec1, Vector3 vec2)
    {
        return (vec1.x * vec2.z) - (vec2.x * vec1.z);
    }
}
