using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    List<Renderer> renderers = new List<Renderer>();

    void Start()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        Color c = new Color();

        foreach (Renderer r in renderers)
        {
            c.r = Random.Range(0.0f, 1.0f);
            c.g = Random.Range(0.0f, 1.0f);
            c.b = Random.Range(0.0f, 1.0f);
            block.SetColor("_Color", c);

            r.SetPropertyBlock(block);
        }
    }
}
