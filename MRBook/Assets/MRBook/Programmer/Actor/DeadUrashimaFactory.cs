using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class DeadUrashimaFactory : MonoBehaviour
{
    [SerializeField]
    Transform slider = null;

    [SerializeField]
    GameObject urashimaPrefab = null;

    [SerializeField]
    Transform spownPoint = null;

    public void StartFactory(int urashimaNum)
    {
        StartCoroutine(FactroyUrashima(urashimaNum));
    }

    IEnumerator FactroyUrashima(int urashimaNum)
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        for(int i = 0;i< urashimaNum;i++)
        {
            Instantiate(urashimaPrefab, spownPoint.position, Quaternion.identity, transform);

            yield return wait;
            yield return StartCoroutine(MoveSlider());
        }
    }

    IEnumerator MoveSlider()
    {
        Vector3 startPosition = slider.localPosition;
        Vector3 endPosition = startPosition + (Vector3.left * 0.6f);
        float duration = 4.0f;

        yield return StartCoroutine(Utilities.FloatLerp(duration, (t) =>
        {
            slider.localPosition = Vector3.Lerp(startPosition, endPosition, t);
        }));
        
        yield return StartCoroutine(Utilities.FloatLerp(duration, (t) =>
        {
            slider.localPosition = Vector3.Lerp(endPosition, startPosition, t);
        }));
    }
}
