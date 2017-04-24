using System;
using UnityEngine;
using GamepadInput;

public class MyInputManager : BaseManager<MyInputManager>
{
    public enum Button { A, B, Y, X, RightShoulder, LeftShoulder, RightStick, LeftStick, Back, Start }

    public enum StickDirection { LeftStickRight, LeftStickLeft, LeftStickUp, LeftStickDown, RightStickRight, RightStickLeft, RightStickUp, RightStickDown }

    public enum Trigger { LeftTrigger, RightTrigger }

    public enum Axis { LeftStick, RightStick, Dpad }

    private static GamepadState[] currentState = new GamepadState[5];
    private static GamepadState[] oldState = new GamepadState[5];

    private static float stickDead = 0.1f;
    private static float triggerDead = 0.05f;

    public static bool IsConnectJoyPad = false;
    private float connectCheckTimer = 0.0f;
    
    private static KeyConfigData keyConfig;

    protected override void Awake()
    {
        //見つけてきたInstanceが自身でない場合はManagerが２つ存在している
        if (this != I)
        {
            Destroy(this);
            return;
        }

        GamePad.GamePadInitialize();
        DontDestroyOnLoad(this.gameObject);
    }

    protected override void Start()
    {
        base.Start();

        keyConfig = Resources.Load<KeyConfigData>("Data/KeyConfigData");

        for (int i = 0; i < currentState.Length; i++)
        {
            currentState[i] = GamePad.GetState((GamePad.Index)i);
            oldState[i] = currentState[i];
        }
    }

    public void Update()
    {
        connectCheckTimer += Time.deltaTime;
        //0.2秒間隔で更新
        if (connectCheckTimer > 0.2f)
        {
            connectCheckTimer = 0.0f;
            string[] temp = Input.GetJoystickNames();
            if (temp.Length != 0)
            {
                IsConnectJoyPad = Input.GetJoystickNames()[0] != "";
            }
        }

        for (int i = 0; i < currentState.Length; i++)
        {
            oldState[i] = currentState[i];
            currentState[i] = GamePad.GetState((GamePad.Index)i);
        }
    }

    public static bool GetButton(Button button, GamePad.Index index = GamePad.Index.One)
    {
        if (IsConnectJoyPad)
        {
            return GamePad.GetButton((GamePad.Button)button, index);
        }

        KeyConfigData.ButtonKeyConfig buttonConfig = null;
        foreach(KeyConfigData.ButtonKeyConfig config in keyConfig.buttonConfigList)
        {
            if (config.buttonName == button) buttonConfig = config;
        }
        
        if (buttonConfig == null) return false;
        if (buttonConfig.code == KeyCode.None) return false;

        bool keyDown = false;

        if (Input.GetKey(buttonConfig.code)) keyDown = true;

        if(buttonConfig.sub != null)
        {
            for(int i = 0;i< buttonConfig.sub.Count;i++)
            {
                if (Input.GetKey(buttonConfig.sub[i])) keyDown = true;
            }
        }

        return keyDown;
    }

    public static bool GetButtonDown(Button button, GamePad.Index index = GamePad.Index.One)
    {
        if (IsConnectJoyPad)
        {
            return GamePad.GetButtonDown((GamePad.Button)button, index);
        }

        KeyConfigData.ButtonKeyConfig buttonConfig = null;
        foreach (KeyConfigData.ButtonKeyConfig config in keyConfig.buttonConfigList)
        {
            if (config.buttonName == button) buttonConfig = config;
        }

        if (buttonConfig == null) return false;
        if (buttonConfig.code == KeyCode.None) return false;

        bool keyDown = false;

        if (Input.GetKeyDown(buttonConfig.code)) keyDown = true;

        if (buttonConfig.sub != null)
        {
            for (int i = 0; i < buttonConfig.sub.Count; i++)
            {
                if (Input.GetKeyDown(buttonConfig.sub[i])) keyDown = true;
            }
        }

        return keyDown;
    }

    public static bool GetButtonUp(Button button, GamePad.Index index = GamePad.Index.One)
    {
        if (IsConnectJoyPad)
        {
            return GamePad.GetButtonUp((GamePad.Button)button, index);
        }

        KeyConfigData.ButtonKeyConfig buttonConfig = null;
        foreach (KeyConfigData.ButtonKeyConfig config in keyConfig.buttonConfigList)
        {
            if (config.buttonName == button) buttonConfig = config;
        }

        if (buttonConfig == null) return false;
        if (buttonConfig.code == KeyCode.None) return false;

        bool keyUp = false;

        if (Input.GetKeyUp(buttonConfig.code)) keyUp = true;

        if (buttonConfig.sub != null)
        {
            for (int i = 0; i < buttonConfig.sub.Count; i++)
            {
                if (Input.GetKeyUp(buttonConfig.sub[i])) keyUp = true;
            }
        }

        return keyUp;
    }
    
    public static Vector2 GetAxis(Axis axis, GamePad.Index index = GamePad.Index.One)
    {
        if (IsConnectJoyPad)
        {
            return GamePad.GetAxis((GamePad.Axis)axis, index);
        }
        
        Vector2 axisXY = Vector2.zero;
        KeyConfigData.AxisKeyConfig axisKeyConfig = null;
        foreach(KeyConfigData.AxisKeyConfig config in keyConfig.axisConfigList)
        {
            if (config.axisName == axis) axisKeyConfig = config;
        }

        if (axisKeyConfig == null) return Vector2.zero;

        if(axisKeyConfig.xAxisName != "null")
            axisXY.x = Input.GetAxis(axisKeyConfig.xAxisName);
        if (axisKeyConfig.yAxisName != "null")
            axisXY.y = Input.GetAxis(axisKeyConfig.yAxisName);

        return axisXY;
    }

    public static float GetTrigger(Trigger trigger, GamePad.Index index = GamePad.Index.One)
    {
        if (IsConnectJoyPad)
        {
            return GamePad.GetTrigger((GamePad.Trigger)trigger, index);
        }
        
        float triggerValue = 0.0f;

        KeyConfigData.TriggerKeyConfig triggerKeyConfig = null;
        foreach(KeyConfigData.TriggerKeyConfig config in keyConfig.triggerConfigList)
        {
            if (config.triggerName == trigger) triggerKeyConfig = config;
        }

        if (triggerKeyConfig == null) return 0.0f;

        if (Input.GetKey(triggerKeyConfig.code)) triggerValue = 1.0f;

        if (triggerKeyConfig.sub != null)
        {
            for (int i = 0; i < triggerKeyConfig.sub.Count; i++)
            {
                if (Input.GetKey(triggerKeyConfig.sub[i])) triggerValue = 1.0f;
            }
        }

        return triggerValue;
    }

    public static bool IsStickDown(StickDirection direction, GamePad.Index index = GamePad.Index.One)
    {
        Vector2 stick;
        if (direction >= (StickDirection)4)
            stick = GetAxis(Axis.RightStick, index);
        else
            stick = GetAxis(Axis.LeftStick, index);
        float dead = 0.3f;
        switch (direction)
        {
            case StickDirection.LeftStickRight:
            case StickDirection.RightStickRight:
                return stick.x > dead;
            case StickDirection.LeftStickLeft:
            case StickDirection.RightStickLeft:
                return stick.x < -dead;
            case StickDirection.LeftStickUp:
            case StickDirection.RightStickUp:
                return stick.y > dead;
            case StickDirection.LeftStickDown:
            case StickDirection.RightStickDown:
                return stick.y < -dead;
        }
        return false;
    }

    public static bool IsJustStickDown(StickDirection direction, GamePad.Index index = GamePad.Index.One)
    {
        Vector2 stick, oldStick;
        if (direction >= (StickDirection)4)
        {
            stick = currentState[(int)index].rightStickAxis;
            oldStick = oldState[(int)index].rightStickAxis;
        }
        else
        {
            stick = currentState[(int)index].LeftStickAxis;
            oldStick = oldState[(int)index].LeftStickAxis;
        }
        
        switch (direction)
        {
            case StickDirection.LeftStickRight:
            case StickDirection.RightStickRight:
                return stick.x > stickDead && oldStick.x <= stickDead;
            case StickDirection.LeftStickLeft:
            case StickDirection.RightStickLeft:
                return stick.x < -stickDead && oldStick.x >= -stickDead;
            case StickDirection.LeftStickUp:
            case StickDirection.RightStickUp:
                return stick.y > stickDead && oldStick.y <= stickDead;
            case StickDirection.LeftStickDown:
            case StickDirection.RightStickDown:
                return stick.y < -stickDead && oldStick.y >= -stickDead;
        }
        return false;
    }

    public static bool IsTriggerDown(Trigger trigger, GamePad.Index index = GamePad.Index.One)
    {
        float t = GetTrigger(trigger, index);

        return t > triggerDead;
    }

    public static bool IsJustTriggerDown(Trigger trigger, GamePad.Index index = GamePad.Index.One)
    {
        float t, oldT;

        if (trigger == Trigger.LeftTrigger)
        {
            t = currentState[(int)index].LeftTrigger;
            oldT = oldState[(int)index].LeftTrigger;
        }
        else
        {
            t = currentState[(int)index].RightTrigger;
            oldT = oldState[(int)index].RightTrigger;
        }

        return t > triggerDead && oldT <= triggerDead;
    }

}