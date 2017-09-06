using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class TapToStartMessage : MonoBehaviour, IInputClickHandler
{
    AnchorPositionController anchorController;
    Renderer m_renderer;

    Vector3 offset = new Vector3(0.0f, 0.4f, 0.0f);
    GameObject mainCamera;
    
    bool IsGameStart = false;

    void Start()
    {
        mainCamera = Camera.main.gameObject;
        anchorController = GetComponentInParent<AnchorPositionController>();
        m_renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (IsGameStart) return;

        if (!anchorController.IsMovable)
        {
            m_renderer.enabled = true;
            Vector3 lookVector = transform.position - mainCamera.transform.position;
            transform.LookAt(transform.position + lookVector);
        }
        else
        {
            m_renderer.enabled = false;
        }
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (IsGameStart) return;
        IsGameStart = true;
        anchorController.Hide();
        MainSceneManager.I.GameStart();
    }
}
