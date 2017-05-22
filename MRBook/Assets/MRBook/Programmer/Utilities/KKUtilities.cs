using System.Collections;
using System;
using UnityEngine;

public class KKUtilities
{
    //duration秒後にactionを実行します
    public static IEnumerator Delay(float duration, Action action)
    {
        yield return new WaitForSeconds(duration);
        action.Invoke();
    }

    public static void Delay(float duration, Action action, MonoBehaviour mono)
    {
        mono.StartCoroutine(Delay(duration, action));
    }

    //aとbの間を補間した値を返す
    public static float FloatLerp(float a, float b, float t)
    {
        t = Mathf.Clamp(t, 0, 1);

        return a + ((b - a) * t);
    }

    //与えられたActionにduration秒かけて０→１になる値を毎フレーム渡す
    public static IEnumerator FloatLerp(float duration, Action<float> action)
    {
        float t = 0.0f;

        while (true)
        {
            t += Time.deltaTime;
            action.Invoke(t / duration);
            if (t > duration) break;
            yield return null;
        }
    }

    /// <summary>
    /// ２点間の(Y成分に限定した)角度を返す
    /// </summary>
    public static float GetAngleY(Vector3 vec1, Vector3 vec2)
    {
        Vector3 temp = vec2 - vec1;
        float vecY = temp.y;
        //X方向だけのベクトルに変換
        temp = Vector3.right * temp.magnitude;
        temp.y = vecY;

        return Vector3.Angle(Vector3.right, temp) * 2.0f;
    }

    /// <summary>
    /// 指定した角度の球体座標を返す
    /// </summary>
    /// <param name="longitude">経度</param>
    /// <param name="latitude">緯度</param>
    /// <returns></returns>
    public static Vector3 SphereCoordinate(float longitude, float latitude, float distance)
    {
        Vector3 position = Vector3.zero;

        //重複した計算
        float temp1 = distance * Mathf.Cos(latitude * Mathf.Deg2Rad);
        float temp2 = longitude * Mathf.Deg2Rad;

        position.x = temp1 * Mathf.Sin(temp2);
        position.y = distance * Mathf.Sin(latitude * Mathf.Deg2Rad);
        position.z = temp1 * Mathf.Cos(temp2);

        return position;
    }
}
