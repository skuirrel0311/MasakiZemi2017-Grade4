using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class ClickerTest : MonoBehaviour
{
    GameObject targetObject = null;
    GestureRecognizer gr = null;

    // Use this for initialization
    void Start()
    {
        CreateTargetObject();
        SetupGestureRecognizer();
        MoveTarget(0.5f);
    }

#if UNITY_EDITOR
    void Update()
    {
        MoveTarget(GetMouseInput());
    }
#endif

    void CreateTargetObject()
    {
        targetObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        targetObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        targetObject.transform.parent = Camera.main.transform;
    }

    void SetupGestureRecognizer()
    {
        gr = new GestureRecognizer();
        gr.NavigationUpdatedEvent += NavigationUpdatedEvent;
        gr.StartCapturingGestures();
    }

    float GetMouseInput()
    {
        return Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
    }

    void NavigationUpdatedEvent(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
    {
        float t = normalizedOffset.z * 0.5f + 0.5f;
        MoveTarget(t);
    }

    // t must be a value between 0.0 and 1.0
    void MoveTarget(float t)
    {
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, t + 5.0f));
        targetObject.transform.position = pos;
    }
}