using System.Collections;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    Transform cameraTransform;
    [SerializeField]
    float updateIntervalTime = 0.05f;
    
    float t = 0.0f;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        t += Time.deltaTime;

        if(t > updateIntervalTime)
        {
            Vector3 tempPos;
            tempPos = cameraTransform.position;
            tempPos.y = transform.position.y;
            transform.LookAt(tempPos);
            t = 0.0f;
        }
    }
}
