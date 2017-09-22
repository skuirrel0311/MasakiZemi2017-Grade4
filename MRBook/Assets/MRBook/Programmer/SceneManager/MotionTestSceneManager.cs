using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionTestSceneManager : BaseManager<MotionTestSceneManager>
{
    [SerializeField]
    Animator actorAnimator = null;

    [SerializeField]
    string[] animationNames = null;

    int currentNameIndex = 0;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            currentNameIndex++;

            if (currentNameIndex >= animationNames.Length)
            {
                currentNameIndex = 0;
            }

            actorAnimator.CrossFade(animationNames[currentNameIndex], 0.1f);
        }
    }
}
