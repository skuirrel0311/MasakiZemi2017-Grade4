using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionTestObjController : MonoBehaviour
{
    Transform m_transform;
    Vector3 zeroVec;

    [SerializeField]
    float moveSpeed = 0.1f;
    [SerializeField]
    float rotationSpeed = 600.0f;

    Transform cameraTransform;

    void Awake()
    {
        m_transform = transform;
        zeroVec = Vector3.zero;
    }

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Vector3 movement = GetInputVec() * moveSpeed * Time.deltaTime;

        if(movement != zeroVec)
        {
            Quaternion cameraRotation;
            cameraRotation = Quaternion.Euler(0.0f, cameraTransform.eulerAngles.y, 0.0f);
            
            movement = cameraRotation * movement;
        }

        //回転
        //今向く方向
        Vector3 forward = Vector3.Slerp(
            m_transform.forward,
            movement,
            rotationSpeed * Time.deltaTime / Vector3.Angle(m_transform.forward, movement)
        );

        m_transform.LookAt(transform.position + forward);

        //移動
        m_transform.Translate(movement, Space.World);
    }

    Vector3 GetInputVec()
    {
        Vector3 vec = zeroVec;

        if (Input.GetKey(KeyCode.A)) vec.x = -1.0f;
        if (Input.GetKey(KeyCode.D)) vec.x = 1.0f;
        if (Input.GetKey(KeyCode.W)) vec.z = 1.0f;
        if (Input.GetKey(KeyCode.S)) vec.z = -1.0f;

        return vec;
    }
}
