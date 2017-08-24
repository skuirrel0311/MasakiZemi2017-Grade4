using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class HoloText : MonoBehaviour , IHoloUI
{
    [SerializeField]
    TextMesh mesh = null;
    
    public string CurrentText
    {
        get { return mesh.text; }
        set
        {
            if (mesh.text.Equals(value)) return;

            mesh.text = value;
        }
    }

    public Color Color
    {
        get
        {
            return mesh.color;
        }
        set
        {
            mesh.color = value;
        }
    }
}
