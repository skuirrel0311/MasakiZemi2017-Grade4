using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField]
    public Transform targetObject = null;

    [SerializeField]
    float distance = 1.0f;    //カメラとターゲットの距離
    [SerializeField]
    float rotationSpeedX = 150.0f;
    [SerializeField]
    float rotationSpeedY = 100.0f;
    [SerializeField]
    float zoomSpeed = 0.01f;

    /// <summary>
    /// 緯度
    /// </summary>
    [SerializeField]
    float latitude = 15.0f;
    /// <summary>
    /// 経度
    /// </summary>
    [SerializeField]
    float longitude = 180.0f;

    /// <summary>
    /// 緯度の最大値
    /// </summary>
    [SerializeField]
    float maxLatitude = 89.0f;
    /// <summary>
    /// 緯度の最小値
    /// </summary>
    [SerializeField]
    float minLatitude = -85.0f;
    /// <summary>
    /// Slerpを開始する緯度の値
    /// </summary>
    [SerializeField]
    float startSlerpLatitude = 30.0f;

    //カメラを制御するか？
    public bool IsWork = true;

    [SerializeField]
    Vector3 offset = new Vector3(0, 0, 0);
    [SerializeField]
    Vector3 targetOffset = new Vector3(0, 2.0f, 0);

    void Start()
    {
        //ターゲットが入ってなかったらプレイヤーを探す
        if (targetObject == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null) targetObject = obj.transform;
        }
        if (targetObject == null)
        {
            GameObject obj = GameObject.Find("Player");
            if (obj != null) targetObject = obj.transform;
        }
    }

    void Update()
    {
        if (!IsWork) return;
        if (Input.GetKey(KeyCode.Z)) distance -= zoomSpeed;
        if (Input.GetKey(KeyCode.X)) distance += zoomSpeed;

        //右スティックで回転
        Vector2 rightStick = GetInputVector();
        longitude += rightStick.x * rotationSpeedX * Time.deltaTime;
        // - はお好み
        latitude -= rightStick.y * rotationSpeedY * Time.deltaTime;

        //経度には制限を掛ける
        latitude = Mathf.Clamp(latitude, minLatitude, maxLatitude);
        longitude = longitude % 360.0f;

        if (targetObject == null)
            SphereCameraControl(Vector3.zero);
        else
            SphereCameraControl(targetObject.position);
    }

    Vector2 GetInputVector()
    {
        Vector2 rightStick;
        rightStick = Vector2.zero;
        if (Input.GetKey(KeyCode.LeftArrow)) rightStick.x = -1;
        if (Input.GetKey(KeyCode.RightArrow)) rightStick.x = 1;
        if (Input.GetKey(KeyCode.UpArrow)) rightStick.y = 1;
        if (Input.GetKey(KeyCode.DownArrow)) rightStick.y = -1;
        return rightStick;
    }

    void SphereCameraControl(Vector3 targetPosition)
    {
        if (latitude < startSlerpLatitude)
        {
            //リープ開始
            Vector3 vec1 = SphereCoordinate(longitude, startSlerpLatitude, distance);
            //リープ終了時の座標
            Vector3 vec2 = SphereCoordinate(longitude, minLatitude, distance);
            vec2.y = 0.0f;

            float t;
            //(開始位置からの移動量) / (全体の移動量) = 0 ～ 1
            if (latitude >= 0.0f)
                t = (startSlerpLatitude - latitude) / (-minLatitude + startSlerpLatitude);
            else
                t = ((-latitude) + startSlerpLatitude) / (-minLatitude + startSlerpLatitude);

            transform.position = targetPosition + Vector3.Slerp(vec1, vec2, t);
        }
        else
        {
            //カメラが地面にめり込まない場合は球体座標をそのまま使う
            transform.position = targetPosition + SphereCoordinate(longitude, latitude, distance) + offset;
        }

        transform.LookAt(targetPosition + targetOffset);
    }

    /// <summary>
    /// 指定した角度の球体座標を返します
    /// </summary>
    /// <param name="longitude">経度</param>
    /// <param name="latitude">緯度</param>
    /// <returns></returns>
    public static Vector3 SphereCoordinate(float longitude, float latitude, float distance)
    {
        Vector3 cameraPosition = Vector3.zero;

        //重複した計算
        float temp1 = distance * Mathf.Cos(latitude * Mathf.Deg2Rad);
        float temp2 = longitude * Mathf.Deg2Rad;

        cameraPosition.x = temp1 * Mathf.Sin(temp2);
        cameraPosition.y = distance * Mathf.Sin(latitude * Mathf.Deg2Rad);
        cameraPosition.z = temp1 * Mathf.Cos(temp2);

        return cameraPosition;
    }
}
