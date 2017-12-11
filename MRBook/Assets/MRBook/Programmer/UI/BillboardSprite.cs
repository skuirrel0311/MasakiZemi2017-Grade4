using System.Collections;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    Transform cameraTransform;
    [SerializeField]
    float updateIntervalTime = 0.05f;

    void Start()
    {
        cameraTransform = Camera.main.transform;

        StartCoroutine(Billboard());
    }

    IEnumerator Billboard()
    {
        WaitForSeconds wait = new WaitForSeconds(updateIntervalTime);
        Vector3 tempPos;

        while(true)
        {
            tempPos = cameraTransform.position;
            tempPos.y = transform.position.y;
            transform.LookAt(tempPos);

            yield return wait;
        }
    }
}
