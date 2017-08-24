using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestObject : MonoBehaviour
{
    public void ShowMessage()
    {
        HoloWindow.I.Show("警告", "メモリが不足しています");
    }
}
