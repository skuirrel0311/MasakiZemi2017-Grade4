using System.Collections;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    Transform cameraTransform;
    [SerializeField]
    float updateIntervalTime = 0.05f;
    
    float t = 0.0f;

    Transform CameraTransform
    {
        get
        {
            if(cameraTransform == null)
            {
                cameraTransform = Camera.main.transform;
            }
            return cameraTransform;
        }
    }

    void Update()
    {
        t += Time.deltaTime;

        if(t > updateIntervalTime)
        {
            LookTarget();
        }
    }

    public void LookTarget()
    {
        Vector3 tempPos;
        tempPos = CameraTransform.position;
        tempPos.y = transform.position.y;
        transform.LookAt(tempPos);
        t = 0.0f;
    }
}
