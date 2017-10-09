using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MenuTest : EditorWindow
{
    int index = 0;
    Vector2 scroll;

    void OnGUI()
    {
        scroll = EditorGUILayout.BeginScrollView(scroll);

        index = GUIExtension.Menu(new List<string> {
            "水素",
            "ヘリウム",
            "リチウム",
            "ベリリウム",
            "ホウ素",
            "炭素",
            "窒素",
            "酸素",
            "フッ素",
            "ネオン",
        }, index);

        EditorGUILayout.EndScrollView();
    }

    [MenuItem("Window/MenuTest")]
    public static void ShowWindow()
    {
        GetWindow<MenuTest>();
    }
}

public static class GUIExtension
{

    public static int Menu(List<string> textList, int index)
    {
        for (int i = 0; i < textList.Count; i++)
        {
            bool flag = GUILayout.Toggle(index == i, textList[i], "OL Elem");
 
            if (flag != (index == i))
            {
                return i;
            }
        }
 
        return index;
    }
}
