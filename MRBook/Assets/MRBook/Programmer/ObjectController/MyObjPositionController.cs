using UnityEngine;
using UnityEngine.AI;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;

[RequireComponent(typeof(Interpolator))]
[RequireComponent(typeof(NavMeshAgent))]
public class MyObjPositionController : MyInputHandler
{
    //手が首からどれだけ離れているか
    float handDistance = 0.0f;
    //オブジェクトが首からどれだけ離れているか
    float objDistance = 0.0f;
    //Gazeの衝突点とオブジェクトの位置の差
    Vector3 objPositionOffset = Vector3.zero;
    //手からGazeの衝突点への回転量
    Quaternion gazeAngleOffset;

    Vector3 draggingPosition;

    protected override void StartDragging()
    {
        base.StartDragging();

        Vector3 gazeHitPosition = GazeManager.Instance.HitInfo.point;
        Vector3 handPosition;
        currentInputSource.TryGetPosition(currentInputSourceID, out handPosition);

        Vector3 headPosition = GetHeadPosition();

        //距離を取得
        handDistance = Vector3.Magnitude(handPosition - headPosition);
        objDistance = Vector3.Magnitude(gazeHitPosition - headPosition);

        //Gazeの衝突点とオブジェクトの位置の差を求める
        objPositionOffset = targetObject.transform.position - gazeHitPosition;
        Vector3 objDirection = Vector3.Normalize(gazeHitPosition - headPosition);
        Vector3 handDirection = Vector3.Normalize(handPosition - headPosition);

        //手からGazeの衝突点への回転量
        gazeAngleOffset = Quaternion.FromToRotation(handDirection, objDirection);

        draggingPosition = gazeHitPosition;
    }

    protected override void UpdateDragging()
    {
        Vector3 currentHandPosition;
        currentInputSource.TryGetPosition(currentInputSourceID, out currentHandPosition);
        Vector3 headPosition = GetHeadPosition();
        
        Vector3 currentHandDirection = Vector3.Normalize(currentHandPosition - headPosition);

        Vector3 objDirection = Vector3.Normalize(gazeAngleOffset * currentHandDirection);

        float currentHandDistance = Vector3.Magnitude(currentHandPosition - headPosition);
        float distanceRatio = currentHandDistance / handDistance;


        draggingPosition = headPosition + (objDirection * (objDistance * distanceRatio));

        interpolator.SetTargetPosition(draggingPosition + objPositionOffset);
    }

    Vector3 GetHeadPosition()
    {
        return mainCameraTransform.position;
    }
}
