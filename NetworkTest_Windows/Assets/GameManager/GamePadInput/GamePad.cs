//Author: Richard Pieterse
//Date: 16 May 2013
//Email: Merrik44@live.com

using UnityEngine;
using System.Collections;

namespace GamepadInput
{

    public static class GamePad
    {

        public enum Button { A, B, Y, X, RightShoulder, LeftShoulder, RightStick, LeftStick, Back, Start }
        public enum Trigger { LeftTrigger, RightTrigger }
        public enum Axis { LeftStick, RightStick, Dpad }
        public enum Index { Any, One, Two, Three, Four }
        public static GamepadState state;

        private static string[] AxisNameList = { "L_XAxis_0", "L_YAxis_0", "L_XAxis_1", "L_YAxis_1", "L_XAxis_2", "L_YAxis_2", "L_XAxis_3", "L_YAxis_3", "L_XAxis_4", "L_YAxis_4",
            "R_XAxis_0", "R_YAxis_0", "R_XAxis_1", "R_YAxis_1", "R_XAxis_2", "R_YAxis_2", "R_XAxis_3", "R_YAxis_3", "R_XAxis_4", "R_YAxis_4",
            "DPad_XAxis_0", "DPad_YAxis_0","DPad_XAxis_1", "DPad_YAxis_1","DPad_XAxis_2", "DPad_YAxis_2","DPad_XAxis_3", "DPad_YAxis_3","DPad_XAxis_4", "DPad_YAxis_4"};

        private static string[] TriggerNameList = { "TriggersL_0", "TriggersR_0", "TriggersL_1", "TriggersR_1", "TriggersL_2", "TriggersR_2", "TriggersL_3", "TriggersR_3", "TriggersL_4", "TriggersR_4"};

        public static void GamePadInitialize()
        {
            state = new GamepadState();
        }

        public static bool GetButtonDown(Button button, Index controlIndex)
        {
            KeyCode code = GetKeycode(button, controlIndex);
            return Input.GetKeyDown(code);
        }

        public static bool GetButtonUp(Button button, Index controlIndex)
        {
            KeyCode code = GetKeycode(button, controlIndex);
            return Input.GetKeyUp(code);
        }

        public static bool GetButton(Button button, Index controlIndex)
        {
            KeyCode code = GetKeycode(button, controlIndex);
            return Input.GetKey(code);
        }

        /// <summary>
        /// returns a specified axis
        /// </summary>
        /// <param name="axis">One of the analogue sticks, or the dpad</param>
        /// <param name="controlIndex">The controller number</param>
        /// <param name="raw">if raw is false then the controlIndex will be returned with a deadspot</param>
        /// <returns></returns>
        public static Vector2 GetAxis(Axis axis, Index controlIndex, bool raw = false)
        {
            Vector2 axisXY = Vector2.zero;
            int axisNameIndex = ((int)axis * 10) + ((int)controlIndex * 2);
            string xAxisName = AxisNameList[axisNameIndex];
            string yAxisName = AxisNameList[axisNameIndex + 1];

            try
            {
                if (raw)
                {
                    axisXY.x = Input.GetAxisRaw(xAxisName);
                    axisXY.y = -Input.GetAxisRaw(yAxisName);
                }
                else
                {
                    axisXY.x = Input.GetAxis(xAxisName);
                    axisXY.y = -Input.GetAxis(yAxisName);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                Debug.LogWarning("Have you set up all axes correctly? \nThe easiest solution is to replace the InputManager.asset with version located in the GamepadInput package. \nWarning: do so will overwrite any existing input");
            }
            return axisXY;
        }

        public static float GetTrigger(Trigger trigger, Index controlIndex, bool raw = false)
        {
            float triggerValue = 0.0f;
            int triggerNameIndex = ((int)trigger) + ((int)controlIndex * 2);
            string triggerName = TriggerNameList[triggerNameIndex];
            try
            {
                if (raw == false)
                    triggerValue = Input.GetAxis(triggerName);
                else
                    triggerValue = Input.GetAxisRaw(triggerName);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                Debug.LogWarning("Have you set up all axes correctly? \nThe easiest solution is to replace the InputManager.asset with version located in the GamepadInput package. \nWarning: do so will overwrite any existing input");
            }
            return triggerValue;
        }
        

        static KeyCode GetKeycode(Button button, Index controlIndex)
        {
            switch (controlIndex)
            {
                case Index.One:
                    switch (button)
                    {
                        case Button.A: return KeyCode.Joystick1Button0;
                        case Button.B: return KeyCode.Joystick1Button1;
                        case Button.X: return KeyCode.Joystick1Button2;
                        case Button.Y: return KeyCode.Joystick1Button3;
                        case Button.RightShoulder: return KeyCode.Joystick1Button5;
                        case Button.LeftShoulder: return KeyCode.Joystick1Button4;
                        case Button.Back: return KeyCode.Joystick1Button6;
                        case Button.Start: return KeyCode.Joystick1Button7;
                        case Button.LeftStick: return KeyCode.Joystick1Button8;
                        case Button.RightStick: return KeyCode.Joystick1Button9;
                    }
                    break;
                case Index.Two:
                    switch (button)
                    {
                        case Button.A: return KeyCode.Joystick2Button0;
                        case Button.B: return KeyCode.Joystick2Button1;
                        case Button.X: return KeyCode.Joystick2Button2;
                        case Button.Y: return KeyCode.Joystick2Button3;
                        case Button.RightShoulder: return KeyCode.Joystick2Button5;
                        case Button.LeftShoulder: return KeyCode.Joystick2Button4;
                        case Button.Back: return KeyCode.Joystick2Button6;
                        case Button.Start: return KeyCode.Joystick2Button7;
                        case Button.LeftStick: return KeyCode.Joystick2Button8;
                        case Button.RightStick: return KeyCode.Joystick2Button9;
                    }
                    break;
                case Index.Three:
                    switch (button)
                    {
                        case Button.A: return KeyCode.Joystick3Button0;
                        case Button.B: return KeyCode.Joystick3Button1;
                        case Button.X: return KeyCode.Joystick3Button2;
                        case Button.Y: return KeyCode.Joystick3Button3;
                        case Button.RightShoulder: return KeyCode.Joystick3Button5;
                        case Button.LeftShoulder: return KeyCode.Joystick3Button4;
                        case Button.Back: return KeyCode.Joystick3Button6;
                        case Button.Start: return KeyCode.Joystick3Button7;
                        case Button.LeftStick: return KeyCode.Joystick3Button8;
                        case Button.RightStick: return KeyCode.Joystick3Button9;
                    }
                    break;
                case Index.Four:

                    switch (button)
                    {
                        case Button.A: return KeyCode.Joystick4Button0;
                        case Button.B: return KeyCode.Joystick4Button1;
                        case Button.X: return KeyCode.Joystick4Button2;
                        case Button.Y: return KeyCode.Joystick4Button3;
                        case Button.RightShoulder: return KeyCode.Joystick4Button5;
                        case Button.LeftShoulder: return KeyCode.Joystick4Button4;
                        case Button.Back: return KeyCode.Joystick4Button6;
                        case Button.Start: return KeyCode.Joystick4Button7;
                        case Button.LeftStick: return KeyCode.Joystick4Button8;
                        case Button.RightStick: return KeyCode.Joystick4Button9;
                    }

                    break;
                case Index.Any:
                    switch (button)
                    {
                        case Button.A: return KeyCode.JoystickButton0;
                        case Button.B: return KeyCode.JoystickButton1;
                        case Button.X: return KeyCode.JoystickButton2;
                        case Button.Y: return KeyCode.JoystickButton3;
                        case Button.RightShoulder: return KeyCode.JoystickButton5;
                        case Button.LeftShoulder: return KeyCode.JoystickButton4;
                        case Button.Back: return KeyCode.JoystickButton6;
                        case Button.Start: return KeyCode.JoystickButton7;
                        case Button.LeftStick: return KeyCode.JoystickButton8;
                        case Button.RightStick: return KeyCode.JoystickButton9;
                    }
                    break;
            }
            return KeyCode.None;
        }

        public static GamepadState GetState(Index controlIndex, bool raw = false)
        {
            state.LeftStickAxis = MyInputManager.GetAxis(MyInputManager.Axis.LeftStick, controlIndex);
            state.rightStickAxis = MyInputManager.GetAxis(MyInputManager.Axis.RightStick, controlIndex);

            state.LeftTrigger = MyInputManager.GetTrigger(MyInputManager.Trigger.LeftTrigger, controlIndex);
            state.RightTrigger = MyInputManager.GetTrigger(MyInputManager.Trigger.RightTrigger, controlIndex);

            return state;
        }

    }

    public struct GamepadState
    {
        public Vector2 LeftStickAxis;
        public Vector2 rightStickAxis;

        public float LeftTrigger;
        public float RightTrigger;
    }

}