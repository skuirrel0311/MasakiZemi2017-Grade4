using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtBookDoor : ArtBookGimmick
{
    [SerializeField]
    GameObject arrowSprite = null;
    [SerializeField]
    float speed = 0.01f;

    Vector3 cameraPosition;
    Vector3 oldPosition;

    protected override void Start()
    {
        cameraPosition = transform.position + transform.up;
        oldPosition = (maker.child.position - cameraPosition).normalized;
        base.Start();
    }

    protected override void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.D))
        {
            isHide = !isHide;
            m_renderer.enabled = !isHide;
            if(!isHide) AkSoundEngine.PostEvent("Eye", gameObject);
            NotificationManager.I.ShowMessage("ドアが" + (isHide ? "開いた" : "閉じた"), 1.0f);
        }
        
#else
        //初回だけ弾く
        if (!maker.IsVisuabled) return;

        if (arrowSprite.activeSelf != maker.IsVisuable)
        {
            //if(maker.IsVisuable) AkSoundEngine.PostEvent("Eye", gameObject);
            arrowSprite.SetActive(maker.IsVisuable);
        }

        if(maker.IsVisuable){
            Vector3 currentPosition = (maker.child.position - cameraPosition).normalized;
            float value = (oldPosition.x * currentPosition.z) - (currentPosition.x * oldPosition.z);
            transform.position += transform.right * value * speed;
            oldPosition = currentPosition;
        }
#endif

    }


}
