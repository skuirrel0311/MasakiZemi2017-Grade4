using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtBookDoor : ArtBookGimmick
{
    [SerializeField]
    GameObject arrowSprite = null;

    [SerializeField]
    Transform openPositionTransform = null;
    float maxCrossValue;

    Vector3 cameraPosition;

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
            if(!isHide) AkSoundEngine.PostEvent("Eye", gameObject);
            NotificationManager.I.ShowMessage("ドアが" + (isHide ? "開いた" : "閉じた"), 1.0f);
        }

#else
        //初回だけ弾く
        if (!maker.IsVisuabled) return;

        if (arrowSprite.activeSelf != maker.IsVisuable)
        {
            if(maker.IsVisuable) AkSoundEngine.PostEvent("Eye", gameObject);
            arrowSprite.SetActive(maker.IsVisuable);
        }

        if(maker.IsVisuable)
        {
            cameraPosition = transform.position + transform.up;
            Vector3 vec1 = (original.transform.position - cameraPosition).normalized;
            Vector3 vec2 = (maker.child.position - cameraPosition).normalized;
            float crossValue = GetCrossValue(vec1, vec2);
            float t = crossValue / maxCrossValue;
            transform.position = Vector3.Lerp(original.transform.position, openPositionTransform.position, t);
        }
#endif

    }

    float GetCrossValue(Vector3 vec1, Vector3 vec2)
    {
        return (vec1.x * vec2.z) - (vec2.x * vec1.z);
    }
}
