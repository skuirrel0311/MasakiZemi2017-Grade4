using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTextController : MonoBehaviour
{
    public enum Mode { Track, Statics}

    Mode currentMode;
    Transform mainCameraTransform;
    Transform MainCameraTransform
    {
        get
        {
            if (mainCameraTransform == null) mainCameraTransform = Camera.main.transform;
            return mainCameraTransform;
        }
    }

    [SerializeField]
    Transform staticsPosition = null;

    void Start()
    {
        MainSceneManager sceneManager = MainSceneManager.I;

        //todo:ページの読み込みが終わった時にTrackになるように設定する
    }

    public void ChangeMode(Mode mode)
    {
        if (currentMode == mode) return;

        currentMode = mode;

        switch(mode)
        {
            case Mode.Statics:
                transform.parent = MainGameUIController.I.transform;
                transform.position = staticsPosition.position;
                transform.rotation = staticsPosition.rotation;
                break;
            case Mode.Track:
                transform.parent = MainCameraTransform;
                transform.position = new Vector3(0.0f, 0.0f, 2.0f);
                transform.rotation = Quaternion.identity;
                break;
        }
    }
}
