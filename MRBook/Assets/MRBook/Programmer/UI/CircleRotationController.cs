using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotationController : MonoBehaviour {

    [SerializeField]
    float rotVal = 15.0f;


    [SerializeField]
    float interval = 0.25f;

    Transform m_transform;
    Vector3 rot;
    float t;
    
    void Start()
    {
        m_transform = transform;
        rot = Vector3.zero;
        t = 0.0f;
    }

    void Update()
    {
        t += Time.deltaTime;

        if (t < interval) return;

        t = 0.0f;
        rot.y += rotVal;
        if (rot.y >= 360.0f)
        {
            rot.y = 0.0f;
        }
        m_transform.rotation = Quaternion.Euler(rot);
    }
}
