using UnityEngine;

public class CircleController : MonoBehaviour
{
    public enum CircleState { Normal, DontPut }
    
    [SerializeField]
    SpriteRenderer spriteRenderer = null;

    [SerializeField]
    Sprite[] sprites = null;

    [SerializeField]
    float rotVal = 15.0f;
    
    [SerializeField]
    float interval = 0.25f;

    Transform m_transform;
    Vector3 rot;
    float t;

    CircleState currentState = CircleState.Normal;
    
    void Start()
    {
        m_transform = transform;
        rot = transform.eulerAngles;
        t = 0.0f;
    }

    void Update()
    {
        if (currentState == CircleState.DontPut) return;

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

    public void ChangeSprite(CircleState state)
    {
        if (state == currentState) return;
        currentState = state;

        spriteRenderer.sprite = sprites[(int)currentState];
    }
}
