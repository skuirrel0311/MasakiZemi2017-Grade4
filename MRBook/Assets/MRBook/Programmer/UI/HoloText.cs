using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class HoloText : MonoBehaviour
{
    TextMesh mesh;
    public string CurrentText
    {
        get { return mesh.text; }
        set
        {
            if (mesh.text == value) return;

            mesh.text = value;
        }
    }

    protected virtual void Awake()
    {
        mesh = GetComponent<TextMesh>();
    }
}
