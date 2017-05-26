using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : BaseManager<NotificationManager>
{
    [SerializeField]
    TextMesh textMesh = null;

    bool isView = false;

    [SerializeField]
    Transform popUpPosition = null;

    Coroutine messageCoroutine;

    public void ShowMessage(string message, float viewTime = 2.0f)
    {
        textMesh.text = message;
        textMesh.gameObject.SetActive(true);

        textMesh.transform.position = popUpPosition.position;
        textMesh.transform.rotation = popUpPosition.rotation;

        if(isView)
        {
            StopCoroutine(messageCoroutine);
        }

        isView = true;
        messageCoroutine = StartCoroutine(KKUtilities.Delay(viewTime, () =>
        {
            isView = false;
            textMesh.gameObject.SetActive(false);
        }));
    }
}
