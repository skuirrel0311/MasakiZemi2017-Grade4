﻿using UnityEngine;
using KKUtilities;

public class UnderTargetMaker : MonoBehaviour
{
    [SerializeField]
    CircleController circleController = null;
    [SerializeField]
    ParticleSystem dottedLine = null;

    HoloObject targetObject;

    BaseObjInputHandler.MakerType currentMakerType = BaseObjInputHandler.MakerType.None;

    public void InitializeMaker(HoloObject putObj, RaycastHit underObj, bool isHit)
    {
        targetObject = putObj;
        circleController.Init();

        if (isHit)
        {
            circleController.transform.position = underObj.point + (Vector3.up * 0.01f);
            Utilities.Delay(1, ()=> SetMakerEnable(true),this);
        }
        else
        {
            SetMakerEnable(false);
        }
    }

    public void SetMakerEnable(bool enable)
    {
        circleController.gameObject.SetActive(enable);
        dottedLine.gameObject.SetActive(enable);
    }

    public void UpdateMaker(BaseObjInputHandler.MakerType makerType, RaycastHit underObj)
    {
        SetMakerType(makerType);

        circleController.SetState(makerType);

        if (makerType == BaseObjInputHandler.MakerType.Normal)
        {
            circleController.transform.position = underObj.point + (Vector3.up * 0.01f);
        }
        else
        {
            circleController.transform.position = underObj.point + (Vector3.up * 0.1f);
        }

        dottedLine.transform.position = targetObject.transform.position;
    }

    void SetMakerType(BaseObjInputHandler.MakerType makerTYpe)
    {
        if (currentMakerType == makerTYpe) return;

        currentMakerType = makerTYpe;

        if (makerTYpe == BaseObjInputHandler.MakerType.None)
        {
            SetMakerEnable(false);
        }
        else
        {
            SetMakerEnable(true);
        }
    }
}
