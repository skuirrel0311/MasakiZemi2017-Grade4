using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideArrow : MonoBehaviour
{
    [SerializeField]
    Transform startPoint = null;

    [SerializeField]
    Transform wayPoint = null;

    [SerializeField]
    Transform endPoint = null;

    [SerializeField]
    Transform emitter = null;

    [SerializeField]
    float speed = 1.0f;

    [SerializeField]
    float waitTime = 2.0f;

    Coroutine moveEmitterCoroutine;

    void Start()
    {
        moveEmitterCoroutine = StartCoroutine(MoveEmitter());

        MainSceneManager.I.OnPlayPage += OnPlay;
    }

    void OnPlay()
    {
        emitter.gameObject.SetActive(false);
        StopCoroutine(moveEmitterCoroutine);
        MainSceneManager.I.OnPlayPage -= OnPlay;
    }

    IEnumerator MoveEmitter()
    {
        float t = 0.0f;
        float temp = 0.0f;

        emitter.position = startPoint.position;
        yield return null;
        emitter.gameObject.SetActive(true);

        while (true)
        {
            t += Time.deltaTime;
            temp = t / speed;
            emitter.position = GetBezierCurvePoint(startPoint.position, wayPoint.position, endPoint.position, temp);
            
            if (temp > 1.0f)
            {
                t = 0.0f;


                yield return new WaitForSeconds(waitTime);

                emitter.gameObject.SetActive(false);

                yield return null;

                emitter.gameObject.SetActive(true);
            }
            else
            {
                yield return null;
            }
        }
    }

    Vector3 GetBezierCurvePoint(Vector3 point1, Vector3 point2, Vector3 point3, float t)
    {
        return Vector3.Lerp(Vector3.Lerp(point1, point2, t), Vector3.Lerp(point2, point3, t), t);
    }
}
