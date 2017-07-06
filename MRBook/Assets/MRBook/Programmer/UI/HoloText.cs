using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class HoloText : MonoBehaviour
{
    TextMesh mesh;
    string currentText;
    public string CurrentText
    {
        get { return currentText; }
        set
        {
            if (currentText == value) return;
            currentText = value;
            if (mesh != null) mesh.text = currentText;
        }
    }

    protected virtual void Start()
    {
        mesh = GetComponent<TextMesh>();
        CurrentText = mesh.text;
    }
}
