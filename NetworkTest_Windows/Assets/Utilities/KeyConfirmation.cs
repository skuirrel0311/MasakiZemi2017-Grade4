using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KeyConfirmation : MonoBehaviour
{
    int buttonLength = 0;
    MyInputManager.Button buttonName;
    int stickButtonLength = 0;
    MyInputManager.StickDirection stickButtonName;

    void Start()
    {
        buttonLength = Enum.GetValues(typeof(MyInputManager.Button)).Length;
        stickButtonLength = Enum.GetValues(typeof(MyInputManager.StickDirection)).Length;
    }
    void Update()
    {
        for(int i = 0;i < buttonLength;i++)
        {
            buttonName = (MyInputManager.Button)i;
            if (MyInputManager.GetButtonDown(buttonName)) Debug.Log("push " + buttonName + " button");
        }

        for(int i = 0;i< stickButtonLength;i++)
        {
            stickButtonName = (MyInputManager.StickDirection)i;
            if (MyInputManager.IsJustStickDown(stickButtonName)) Debug.Log("push " + stickButtonName);
        }

        if (MyInputManager.IsJustTriggerDown(MyInputManager.Trigger.LeftTrigger)) Debug.Log("push LeftTrigger");
        if (MyInputManager.IsJustTriggerDown(MyInputManager.Trigger.RightTrigger)) Debug.Log("push RightTrigger");

        float leftTrigger = MyInputManager.GetTrigger(MyInputManager.Trigger.LeftTrigger);
        if (leftTrigger != 0.0f) Debug.Log("leftTrigger = " + leftTrigger);
        float rightTrigger = MyInputManager.GetTrigger(MyInputManager.Trigger.RightTrigger);
        if (rightTrigger != 0.0f) Debug.Log("leftTrigger = " + rightTrigger);

        Vector2 leftStick = MyInputManager.GetAxis(MyInputManager.Axis.LeftStick);
        if (leftStick != Vector2.zero) Debug.Log("leftStick = " + leftStick);
        Vector2 rightStick = MyInputManager.GetAxis(MyInputManager.Axis.RightStick);
        if (rightStick != Vector2.zero) Debug.Log("rightStick = " + rightStick);
        Vector2 dPad = MyInputManager.GetAxis(MyInputManager.Axis.Dpad);
        if (dPad != Vector2.zero) Debug.Log("dPad = " + dPad);
    }
}
