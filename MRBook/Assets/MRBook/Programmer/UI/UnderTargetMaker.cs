using UnityEngine;
using KKUtilities;

public class UnderTargetMaker : MonoBehaviour
{
    [SerializeField]
    CircleController circleController = null;
    [SerializeField]
    ParticleSystem dottedLine = null;

    HoloObject targetObject;

    BaseObjInputHandler.MakerType currentMakerType = BaseObjInputHandler.MakerType.None;

    public void InitializeMaker(HoloObject putObj, RaycastHit underObj, BaseObjInputHandler.MakerType makerType)
    {
        targetObject = putObj;
        circleController.Init();

        if (makerType != BaseObjInputHandler.MakerType.None)
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

    void SetMakerType(BaseObjInputHandler.MakerType makerType)
    {
        if (currentMakerType == makerType) return;

        currentMakerType = makerType;
        circleController.SetState(makerType);

        if (makerType == BaseObjInputHandler.MakerType.None)
        {
            SetMakerEnable(false);
        }
        else
        {
            SetMakerEnable(true);
        }
    }
}
