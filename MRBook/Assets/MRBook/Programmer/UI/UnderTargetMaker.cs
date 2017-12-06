using UnityEngine;

public class UnderTargetMaker : MonoBehaviour
{
    [SerializeField]
    CircleController circleController = null;
    [SerializeField]
    ParticleSystem dottedLine = null;

    public void ShowMaker(HoloObject putObj, RaycastHit underObj)
    {
        circleController.transform.position = underObj.point + (Vector3.up * 0.001f);
        dottedLine.transform.position = putObj.transform.position;

        circleController.gameObject.SetActive(true);
        dottedLine.gameObject.SetActive(true);
    }

    public void HideMaker()
    {
        circleController.gameObject.SetActive(false);
        dottedLine.gameObject.SetActive(false);
    }

    public void SetMaker(BaseObjInputHandler.MakerType makerType, HoloObject putObj, RaycastHit underObj)
    {
        switch(makerType)
        {
            case BaseObjInputHandler.MakerType.None:
                HideMaker();
                break;
            case BaseObjInputHandler.MakerType.DontPut:
            case BaseObjInputHandler.MakerType.DontPresentItem:
                ShowMaker(putObj, underObj);
                circleController.ChangeSprite(CircleController.CircleState.DontPut);
                break;
            case BaseObjInputHandler.MakerType.PresentItem:
                HideMaker();
                break;
            case BaseObjInputHandler.MakerType.Normal:
                circleController.ChangeSprite(CircleController.CircleState.Normal);
                ShowMaker(putObj, underObj);
                break;
        }
    }
}
