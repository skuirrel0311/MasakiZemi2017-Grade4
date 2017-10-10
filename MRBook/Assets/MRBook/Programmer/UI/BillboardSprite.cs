using System.Collections;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;

        StartCoroutine(Billboard());
    }

    IEnumerator Billboard()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
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
