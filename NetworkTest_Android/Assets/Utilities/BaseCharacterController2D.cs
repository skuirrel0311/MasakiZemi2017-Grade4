using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterController2D : MonoBehaviour
{
    /*パラメーター*/
    [SerializeField]
    protected float moveSpeed = 4.0f;
    [SerializeField]
    protected float rotateSpeed = 600.0f;
    [SerializeField]
    protected float jumpPower = 5.0f;
    [SerializeField]
    protected int maxHp = 10;
    public int hp;

    /*ステータス*/
    protected enum CharacterState { Grounding, Jumping, Falling, Landing, Damege, Dead }
    [SerializeField]
    protected CharacterState currentState = CharacterState.Grounding;

    [System.NonSerialized]
    public Vector3 velocity = Vector3.zero;
    //慣性
    protected float iner = 1.0f;
    //空気抵抗
    protected float airResistance = 0.98f;
    
    protected float landStartTime = 0.0f;
    
    Transform[] underCollider = new Transform[3];
    [SerializeField]
    LayerMask floorLayerMask = 0;
    protected bool isOnGround = true;

    /*キャッシュ*/
    protected Rigidbody body;
    protected Animator anim;

    protected virtual void Start()
    {
        hp = maxHp;
        underCollider[0] = transform.FindChild("UnderCollider/UnderCollider_B").GetComponent<Transform>();
        underCollider[1] = transform.FindChild("UnderCollider/UnderCollider_C").GetComponent<Transform>();
        underCollider[2] = transform.FindChild("UnderCollider/UnderCollider_F").GetComponent<Transform>();

        body = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void FixedUpdate()
    {
        if (currentState == CharacterState.Dead) return;
        
        isOnGround = IsNearGround();

        StateUpdate();
        
        if (currentState == CharacterState.Jumping)
        {
            iner *= airResistance;
            velocity *= iner;
        }
        else
        {
            iner = 1.0f;
        }

        if(currentState == CharacterState.Damege)
        {
            velocity *= 0.1f;
        }

        //このVelocityのYの値は常に０が入っている
        ActionMove(velocity * moveSpeed * Time.deltaTime);

        if (transform.position.y <= -2.0f) Dead();
    }

    void StateUpdate()
    {
        switch(currentState)
        {
            case CharacterState.Grounding:
                if (!isOnGround)
                {
                    anim.SetTrigger("Falling");
                    currentState = CharacterState.Falling;
                }
                break;
            case CharacterState.Jumping:
                if (body.velocity.y < 0.1f)
                { 
                    currentState = CharacterState.Falling;
                }
                break;
            case CharacterState.Falling:
                if (IsNearGround(0.4f))
                {
                    landStartTime = Time.fixedTime;
                    anim.SetTrigger("Landing");
                    currentState = CharacterState.Landing;
                }
                break;
            case CharacterState.Landing:
                if (Time.fixedTime - landStartTime > 0.5f || Mathf.Abs(velocity.x) > 0.3f)
                {
                    currentState = CharacterState.Grounding;
                }
                break;
        }
    }

    protected virtual void Update()
    {

    }

    public virtual void ActionMove(Vector3 movement)
    {
        //回転
        //今向く方向
        Vector3 forward = Vector3.Slerp(
            transform.forward,
            movement, 
            rotateSpeed * Time.deltaTime / Vector3.Angle(transform.forward, movement));

        transform.LookAt(transform.position + forward);
        
        //移動
        transform.Translate(movement, Space.World);
    }

    public virtual void ActionJump()
    {
        anim.CrossFade("JumpToTop", 0.1f);
        currentState = CharacterState.Jumping;
        body.velocity = Vector3.up * jumpPower;
    }

    protected bool IsNearGround(float distance = 0.2f)
    {
        Collider[] temp;
        bool isNear = false;
        for (int i = 0; i < underCollider.Length; i++)
        {
            underCollider[i].localPosition = new Vector3(underCollider[i].localPosition.x, distance, underCollider[i].localPosition.z);
            temp = Physics.OverlapSphere(underCollider[i].position, 0.02f, floorLayerMask);
            if (temp.Length == 0) continue;
            isNear = true;
        }

        return isNear;
    }

    public virtual void Damage(int value)
    {
        currentState = CharacterState.Damege;
        hp -= value;
        hp = Mathf.Max(maxHp, hp);

        if (hp <= 0) Dead(true);
    }

    public virtual void Damage(int value, Vector3 power)
    {
        Damage(value);
        body.AddForce(power);
    }

    public virtual void Recovery(int value)
    {
        hp += value;
        hp = Mathf.Min(maxHp, hp);
    }

    public virtual void Dead(bool gameOver = false)
    {
        currentState = CharacterState.Dead;

        if(gameOver)
        {

        }
        else
        {
            LoadSceneManager.I.LoadScene("Main", true, 1.0f, 0.1f);
        }
    }
}
