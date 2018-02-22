using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemText : MonoBehaviour
{
    [SerializeField]
    MyCursor cursor = null;

    [SerializeField]
    HoloText text = null;

    ActorManager actorManager;

    Camera mainCamera;
    Ray ray;
    RaycastHit hit;
    [SerializeField]
    LayerMask layerMask;

    void Start()
    {
        actorManager = ActorManager.I;
        mainCamera = Camera.main;
        if (cursor != null) cursor.onFocuesdObjectChanged += OnFocuesdObjectChanged;
    }

#if UNITY_EDITOR
    void Update()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10.0f, layerMask))
        {
            HoloObject obj = actorManager.GetObject(hit.transform.name);

            if (obj == null || obj.GetActorType != HoloObject.Type.Item)
            {
                text.gameObject.SetActive(false);
                return;
            }

            ShowItemText((HoloItem)obj);
            return;
        }

        text.gameObject.SetActive(false);
    }
#endif

    void OnFocuesdObjectChanged(GameObject preObj, GameObject newObj)
    {
        if (!HoloObjectController.I.canClick || HoloObjectController.I.IsDragging)
        {
            text.gameObject.SetActive(false);
            return;
        }

        if (newObj == null || newObj.tag != "Actor")
        {
            text.gameObject.SetActive(false);
            return;
        }
        
        HoloObject obj = actorManager.GetObject(newObj.name);

        if (obj == null || obj.GetActorType != HoloObject.Type.Item) return;

        ShowItemText((HoloItem)obj);
    }

    void ShowItemText(HoloItem item)
    {
        Vector3 itemTextPosition = item.transform.position;

        //オブジェクトのピポッドによって高さは変わるので制御しきれなそう
        float height = item.transform.lossyScale.x * item.InputHandler.m_collider.size.y * 2.0f;

        //Debug.Log("height = " + height);

        itemTextPosition.y += height + item.itemTextHeight;
        //float scale = item.InputHandler.m_collider.size.x * item.transform.lossyScale.x * 0.1f;
        //Debug.Log(item.name + " = " + item.InputHandler.m_collider.size.x + " * " + item.transform.lossyScale.x + " * 0.1 = " + scale);
        //scale = Mathf.Clamp(scale, 0.02f, 0.08f);

        text.transform.localScale = Vector3.one * 0.015f;
        text.transform.position = itemTextPosition;
        text.CurrentText = item.nameText;
        text.gameObject.SetActive(true);
    }


}
