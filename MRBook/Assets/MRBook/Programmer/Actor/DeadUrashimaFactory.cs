using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;
using RootMotion.Dynamics;

public class DeadUrashimaFactory : MonoBehaviour
{
    [SerializeField]
    GameObject urashimaPrefab = null;

    [SerializeField]
    Transform spownPoint = null;

    PuppetMaster currentUrashima;

    [SerializeField]
    Color startColor = Color.clear;
    [SerializeField]
    Color targetColor = Color.white;

    public void StartFactory(int urashimaNum)
    {
        StartCoroutine(FactroyUrashima(urashimaNum));
    }

    IEnumerator FactroyUrashima(int urashimaNum)
    {
        yield return null;

        WaitForSeconds wait = new WaitForSeconds(0.1f);
        Vector3 instancePosition;
        Quaternion instanceRotation = spownPoint.rotation;

        for (int i = 0; i < urashimaNum; i++)
        {
            instancePosition = spownPoint.position;
            instancePosition.x += (i / 3) * 0.1f;
            instancePosition.z +=(i % 3) * 0.15f;
            GameObject obj = Instantiate(urashimaPrefab, instancePosition, instanceRotation, transform);
            Material mat = obj.GetComponentInChildren<Renderer>().material;
            yield return StartCoroutine(UrashimaFade(obj.transform, mat));
        }
    }

    IEnumerator UrashimaFade(Transform trans, Material material)
    {
        Vector3 startPosition = trans.position + Vector3.up * 0.03f;
        Vector3 targetPosition = trans.position;
        yield return StartCoroutine(Utilities.FloatLerp(0.5f, (t) =>
        {
            trans.position = Vector3.Lerp(startPosition, targetPosition, t);
            material.SetColor("_TintColor", Color.Lerp(startColor, targetColor, t * t));
        }));
    }

    //IEnumerator FactroyUrashima(int urashimaNum)
    //{
    //    WaitForSeconds wait = new WaitForSeconds(0.1f);
    //    for(int i = 0;i< urashimaNum;i++)
    //    {
    //        GameObject obj = Instantiate(urashimaPrefab, spownPoint.position, Quaternion.identity, transform);
    //        currentUrashima = obj.GetComponentInChildren<PuppetMaster>();

    //        yield return wait;
    //        yield return StartCoroutine(MoveSlider());

    //        currentUrashima.state = PuppetMaster.State.Frozen;
    //    }
    //}

    //IEnumerator MoveSlider()
    //{
    //    Vector3 startPosition = slider.localPosition;
    //    Vector3 endPosition = startPosition + (Vector3.left * 0.6f);
    //    float duration = 4.0f;

    //    yield return StartCoroutine(Utilities.FloatLerp(duration, (t) =>
    //    {
    //        slider.localPosition = Vector3.Lerp(startPosition, endPosition, t);
    //    }));

    //    yield return StartCoroutine(Utilities.FloatLerp(duration, (t) =>
    //    {
    //        slider.localPosition = Vector3.Lerp(endPosition, startPosition, t);
    //    }));
    //}
}
