using UnityEngine;

public class MyObjRotationController : MyInputHandler
{
    Vector3 oldHandPosition;
    [SerializeField]
    float rotationSpeed = 360.0f;

    Vector3 targetRotation;

    protected override void StartDragging()
    {
        base.StartDragging();

        oldHandPosition = GetHandPosition().normalized;
    }

    protected override void UpdateDragging()
    {
        Vector3 currentHandPosition = GetHandPosition().normalized;

        //回転量の計算（外積）
        float rotationValue = (oldHandPosition.x * currentHandPosition.z) - (currentHandPosition.x * oldHandPosition.z);
        
        targetRotation = targetObject.transform.localEulerAngles;
        targetRotation.y += rotationValue * rotationSpeed;
        //Y軸回転
        targetObject.transform.localRotation = Quaternion.Euler(targetRotation);


        oldHandPosition = currentHandPosition;        
    }
}
