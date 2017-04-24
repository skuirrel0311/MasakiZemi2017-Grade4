using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : BaseCharacterController2D
{
    GameObject cameraObj = null;
    public float cameraDistance = 5.0f;

    [SerializeField]
    ScreenButton leftScreenButton = null;
    [SerializeField]
    ScreenButton rightScreenButton = null;

    protected override void Start()
    {
        base.Start();
        cameraObj = Camera.main.gameObject;
    }

    protected override void Update()
    {
        GetInput();

        anim.SetFloat("MoveSpeed", Mathf.Abs(velocity.x));

        cameraObj.transform.position = (transform.position - (Vector3.forward * cameraDistance)) + (Vector3.up);
    }

    void GetInput()
    {
        velocity.x = 0.0f;
        if (leftScreenButton.IsPushing) velocity.x -= 1.0f;
        if (rightScreenButton.IsPushing) velocity.x += 1.0f;

        //どっちも押されていたら先に押されていた方を優先する
        if (leftScreenButton.IsPushing && rightScreenButton.IsPushing)
        {
            velocity.x = (leftScreenButton.pushingTime > rightScreenButton.pushingTime) ? -1.0f : 1.0f;
        }

        if (leftScreenButton.IsJustPush)
        {
            if (rightScreenButton.IsPushing) ActionJump();
        }

        if (rightScreenButton.IsJustPush)
        {
            if (leftScreenButton.IsPushing) ActionJump();
        }
    }

    public override void ActionJump()
    {
        if (currentState == CharacterState.Grounding || currentState == CharacterState.Landing)
            base.ActionJump();
    }
}
