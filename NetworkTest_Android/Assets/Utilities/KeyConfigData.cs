using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyConfigData : ScriptableObject
{
    [System.Serializable]
    public class ButtonKeyConfig
    {
        public MyInputManager.Button buttonName { get; private set; }
        public KeyCode code;
        public List<KeyCode> sub = null;

        public ButtonKeyConfig(MyInputManager.Button buttonName)
        {
            this.buttonName = buttonName;
        }

        public ButtonKeyConfig(MyInputManager.Button buttonName, KeyCode code)
        {
            this.buttonName = buttonName;
            this.code = code;
        }
    }
    [System.Serializable]
    public class AxisKeyConfig
    {
        public MyInputManager.Axis axisName { get; private set; }
        public string xAxisName;
        public string yAxisName;

        public AxisKeyConfig(MyInputManager.Axis axisName, string x, string y)
        {
            this.axisName = axisName;
            xAxisName = x;
            yAxisName = y;
        }
    }
    [System.Serializable]
    public class TriggerKeyConfig
    {
        public MyInputManager.Trigger triggerName { get; private set; }
        public KeyCode code;
        public List<KeyCode> sub = null;

        public TriggerKeyConfig(MyInputManager.Trigger triggerName, KeyCode code)
        {
            this.triggerName = triggerName;
            this.code = code;
        }
    }

    public List<ButtonKeyConfig> buttonConfigList { get; private set; }
    public List<AxisKeyConfig> axisConfigList { get; private set; }
    public List<TriggerKeyConfig> triggerConfigList { get; private set; }

    public ButtonKeyConfig a = new ButtonKeyConfig(MyInputManager.Button.A, KeyCode.Space);
    public ButtonKeyConfig b = new ButtonKeyConfig(MyInputManager.Button.B, KeyCode.E);
    public ButtonKeyConfig y = new ButtonKeyConfig(MyInputManager.Button.Y, KeyCode.Slash);
    public ButtonKeyConfig x = new ButtonKeyConfig(MyInputManager.Button.X, KeyCode.Backslash);
    public ButtonKeyConfig rightShoulder = new ButtonKeyConfig(MyInputManager.Button.RightShoulder, KeyCode.LeftShift);
    public ButtonKeyConfig leftShoulder = new ButtonKeyConfig(MyInputManager.Button.LeftShoulder, KeyCode.Q);
    public ButtonKeyConfig rightStickButton = new ButtonKeyConfig(MyInputManager.Button.RightStick);
    public ButtonKeyConfig leftStickButton = new ButtonKeyConfig(MyInputManager.Button.LeftStick);
    public ButtonKeyConfig back = new ButtonKeyConfig(MyInputManager.Button.Back);
    public ButtonKeyConfig start = new ButtonKeyConfig(MyInputManager.Button.Start, KeyCode.Escape);

    public AxisKeyConfig leftStick = new AxisKeyConfig(MyInputManager.Axis.LeftStick, "LeftHorizontal", "LeftVertical");
    public AxisKeyConfig rightStick = new AxisKeyConfig(MyInputManager.Axis.RightStick, "RightHorizontal", "RightVertical");
    public AxisKeyConfig dPad = new AxisKeyConfig(MyInputManager.Axis.Dpad, "null", "null");

    public TriggerKeyConfig leftTrigger = new TriggerKeyConfig(MyInputManager.Trigger.LeftTrigger, KeyCode.K);
    public TriggerKeyConfig rightTrigger = new TriggerKeyConfig(MyInputManager.Trigger.RightTrigger, KeyCode.L);

    public KeyConfigData()
    {
        buttonConfigList = new List<ButtonKeyConfig>();
        buttonConfigList.Add(a);
        buttonConfigList.Add(b);
        buttonConfigList.Add(y);
        buttonConfigList.Add(x);
        buttonConfigList.Add(rightShoulder);
        buttonConfigList.Add(leftShoulder);
        buttonConfigList.Add(rightStickButton);
        buttonConfigList.Add(leftStickButton);
        buttonConfigList.Add(back);
        buttonConfigList.Add(start);

        axisConfigList = new List<AxisKeyConfig>();
        axisConfigList.Add(leftStick);
        axisConfigList.Add(rightStick);
        axisConfigList.Add(dPad);

        triggerConfigList = new List<TriggerKeyConfig>();
        triggerConfigList.Add(leftTrigger);
        triggerConfigList.Add(rightTrigger);
    }
}



