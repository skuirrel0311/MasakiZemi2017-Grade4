using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;


//アイテムを地面に落とすためのやつ
public class ItemDropper : BaseManager<ItemDropper>
{
    Rigidbody m_body;
    SphereCollider m_collider;

    [SerializeField]
    float burstPower = 5.0f;

    OffsetController offsetController;

    protected override void Awake()
    {
        base.Awake();
        m_body = GetComponent<Rigidbody>();
        m_collider = GetComponent<SphereCollider>();
    }

    protected override void Start()
    {
        base.Start();
        offsetController = OffsetController.I;
    }

    public void Drop(HoloObject owner, HoloItem item)
    {
        SetActive(true);
        transform.position = item.transform.position;

        item.transform.parent = transform;
        item.transform.rotation = Quaternion.identity;

        Vector3 burstVec = item.transform.position - owner.transform.position;

        burstVec.y = 0.1f;

        m_body.AddForce(burstVec.normalized * burstPower, ForceMode.VelocityChange);

        MyCoroutine coroutine = new MyCoroutine(MonitorVelocity());
        coroutine.OnCompleted(() =>
        {
            Debug.Log("calm vec");
            item.transform.parent = item.defaultParent;
        });

        StartCoroutine(coroutine);
    }

    IEnumerator MonitorVelocity()
    {
        Vector3 startPosition = transform.position;
        yield return null;
        yield return null;
        
        while(true)
        {
            //Debug.Log("vec = ( " + m_body.velocity.x + " , " + m_body.velocity.y + " , " + m_body.velocity.z + " )");
            if (IsCalmVelocity(m_body.velocity)) break;
            if(IsOverBookHeight())
            {
                Vector3 airPosition = startPosition;
                airPosition.y = offsetController.bookTransform.position.y + 0.5f;
                transform.position = airPosition;
                break;
            }
            yield return null;
        }
        SetActive(false);
    }

    bool IsCalmVelocity(Vector3 vec)
    {
        float border = 0.001f;
        float length = vec.magnitude;
        //Debug.Log("length = " + length);
        return length < border;
    }

    bool IsOverBookHeight()
    {
        return transform.position.y < offsetController.bookTransform.position.y;
    }

    void SetActive(bool active)
    {
        m_body.isKinematic = !active;
        m_collider.enabled = active;
    }
}
