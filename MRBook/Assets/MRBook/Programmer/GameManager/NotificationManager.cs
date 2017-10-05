using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class NotificationManager : BaseManager<NotificationManager>
{
    [SerializeField]
    TextMesh textMesh = null;

    [SerializeField]
    HoloWindow normalWindow = null;

    bool isView = false;

    Coroutine messageCoroutine;

    Vector3 defaultPos;
    Quaternion defaultRot;

    protected override void Start()
    {
        base.Start();

        defaultPos = transform.position;
        defaultRot = transform.rotation;
    }

    private void ShowMessage(string message, float viewTime, bool useDefault)
    {
        textMesh.text = message;
        textMesh.gameObject.SetActive(true);

        if (useDefault)
        {
            transform.position = defaultPos;
            transform.rotation = defaultRot;
        }

        if (isView)
        {
            StopCoroutine(messageCoroutine);
        }

        isView = true;
        messageCoroutine = StartCoroutine(Utilities.Delay(viewTime, () =>
        {
            isView = false;
            textMesh.gameObject.SetActive(false);
        }));

        //toto:消えるときのフェード
    }

    public void ShowMessage(string message, float viewTime = 2.0f)
    {
        ShowMessage(message, viewTime, true);
    }

    public void ShowMessage(string message, Vector3 pos, Quaternion rot, float viewTime = 2.0f)
    {
        transform.position = pos;
        transform.rotation = rot;

        ShowMessage(message, viewTime, false);
    }

    public void ShowMessage(string message, Transform transform, float viewTime = 2.0f)
    {
        ShowMessage(message, transform.position, transform.rotation, viewTime);
    }

    public void SetDefaultTransform(Vector3 pos, Quaternion rot)
    {
        defaultPos = pos;
        defaultRot = rot;
    }

    public void SetDefaultTransform(Transform transform)
    {
        SetDefaultTransform(transform.position, transform.rotation);
    }

    public void ShowDialog(string title, string message, bool autoHide = false, float hideTime = 1.0f)
    {
        normalWindow.Show(title, message, autoHide, hideTime);
    }
}
