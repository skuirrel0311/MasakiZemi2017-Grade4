using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulsGetter : MonoBehaviour
{
    const int waterLayer = 4;
    Rigidbody m_body;

    protected virtual void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer != waterLayer) return;

        if(m_body == null)
        {
            m_body = GetComponent<Rigidbody>();
        }
        float impuls = col.impulse.magnitude;
        Debug.Log("impuls = " + impuls);
        if (impuls < 0.3f)
        {
            Vector3 direction = (transform.position - col.contacts[0].point).normalized;

            Debug.Log("hit direction = (" + direction.x + "," + direction.y + "," + direction.z + ")");

            m_body.AddForce(direction * 10.0f, ForceMode.VelocityChange);


        }
    }
}
