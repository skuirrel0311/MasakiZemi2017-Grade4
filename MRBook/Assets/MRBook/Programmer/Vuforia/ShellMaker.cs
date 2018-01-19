using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellMaker : MonoBehaviour
{
    [SerializeField]
    ShellTracableEventHandler shellFront = null;
    [SerializeField]
    ShellTracableEventHandler shellBack = null;
    [SerializeField]
    GameObject treasureObject = null;

    Transform cameraTransform;

    bool isCloseShell = true;
    bool IsCloseShell
    {
        get
        {
            return isCloseShell;
        }
        set
        {
            if (isCloseShell == value) return;
            if (!IsNearCamera()) return;

            isCloseShell = value;
            ChangeShellState(isCloseShell);
        }
    }

    bool IsNearCamera()
    {
        float distance = Vector3.Distance(cameraTransform.position, treasureObject.transform.position);
        Debug.Log("distance = " + distance);
        return true;
    }

    void Start()
    {
        cameraTransform = Camera.main.transform;

#if !UNITY_EDITOR

        shellFront.onFound += () =>
        {
            IsCloseShell = true;
        };

        shellBack.onFound += () =>
        {
            IsCloseShell = false;
        };
#endif
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            NotificationManager.I.ShowMessage(!IsCloseShell ? "貝殻を開けました" : "貝殻を閉じました");
            IsCloseShell = !IsCloseShell;
        }
    }
#endif

    void ChangeShellState(bool isClose)
    {
        AkSoundEngine.PostEvent("Eye", gameObject);
        treasureObject.gameObject.SetActive(!isClose);
    }
}
