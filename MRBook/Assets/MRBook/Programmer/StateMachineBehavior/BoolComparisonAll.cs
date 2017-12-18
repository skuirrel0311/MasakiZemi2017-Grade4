using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoolComparisonElement
{
    public string flagName;
    public bool boolValue = true;
    public bool isCheckNow = true;
}

public class BoolComparisonAll : BaseStateMachineBehaviour
{
    public BoolComparisonElement[] element;
    public enum Type { And, Or }
    public Type t = Type.And;

    bool[] flaggedArray;

    protected override void OnStart()
    {
        base.OnStart();
        flaggedArray = new bool[element.Length];

        for (int i = 0; i < element.Length; i++)
        {
            flaggedArray[i] = FlagManager.I.GetFlag(element[i].flagName, MainSceneManager.I.currentPageIndex, element[i].isCheckNow) == element[i].boolValue;
            m_animator.SetBool("Flagged" + (i + 1), flaggedArray[i]);
        }


        m_animator.SetBool("Flagged", t == Type.And ? BoolAllTrue() : BoolAnyTrue());
    }

    bool BoolAnyTrue()
    {
        for (int i = 0; i < element.Length; i++)
        {
            if (flaggedArray[i]) return true;
        }

        return false;
    }

    bool BoolAllTrue()
    {
        for (int i = 0; i < element.Length; i++)
        {
            if (!flaggedArray[i]) return false;
        }

        return true;
    }
}
