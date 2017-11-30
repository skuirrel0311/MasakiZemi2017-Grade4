﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class CheckAnimator : EditorWindow
{
    AnimatorController animatorController;
    int animatorNum = 3;

    [MenuItem("MyTools/BakeAnimatorWindow")]
    static void ShowWindow()
    {
        GetWindow<CheckAnimator>();
    }


    void Hoge(string filenName)
    {
        string animatorPath = "Assets/MRBook/PageAnimatorControllers/" + filenName + ".controller";
        RuntimeAnimatorController asset = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(animatorPath);
        animatorController = asset as AnimatorController;
        
        AnimatorBakedData bakedData = CreateInstance<AnimatorBakedData>();

        ChildAnimatorState[] states = animatorController.layers[0].stateMachine.states;
        string layerName = animatorController.layers[0].name;
        int hash;
        string stateName;
        for (int i = 0; i < states.Length; i++)
        {
            stateName = states[i].state.name;
            hash = Animator.StringToHash(layerName + "." + stateName);
            bakedData.AddDictionary(hash, states[i].state.behaviours.Length);
        }

        string stateMachineName;
        for (int i = 0;i< animatorController.layers[0].stateMachine.stateMachines.Length;i++)
        {
            states = animatorController.layers[0].stateMachine.stateMachines[i].stateMachine.states;
            stateMachineName = animatorController.layers[0].stateMachine.stateMachines[i].stateMachine.name;
            for(int j = 0;j< states.Length;j++)
            {
                stateName = states[j].state.name;

                hash = Animator.StringToHash(layerName + "." + stateMachineName + "." + stateName);
                bakedData.AddDictionary(hash, states[j].state.behaviours.Length);
            }
        }

        string assetPath = "Assets/MRBook/Resources/Data/" + filenName + ".asset";
        if (IsThereOriginalFile(filenName))
        {
            //元のファイルは削除
            AssetDatabase.DeleteAsset(assetPath);
            Debug.Log("上書きしました");
        }
        //作成
        AssetDatabase.CreateAsset(bakedData, assetPath);
        //保存
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    bool IsThereOriginalFile(string fileName)
    {
        AnimatorBakedData original = Resources.Load<AnimatorBakedData>("Data/" + fileName);

        return original != null;
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("menu");

        animatorNum = EditorGUILayout.IntField(animatorNum, GUILayout.Width(100));
        string[] strs = new string[animatorNum];

        for (int i = 0; i < animatorNum; i++)
        {
            strs[i] = EditorGUILayout.TextField("Page" + (i + 1) + "Controller", GUILayout.Width(300));
        }
        if (GUILayout.Button("Bake"))
        {
            for (int i = 0; i < animatorNum; i++)
            {
                Hoge(strs[i]);
            }
        }
    }
}
