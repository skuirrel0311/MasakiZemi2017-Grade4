using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GetCurrentSelectObject : Editor 
{
	[MenuItem("Mytools/GetCurrentSelectObject")]
	static void DrawText()
	{
		var asset = ScriptableObject.CreateInstance<ItemTransformDataList> ();

		if (Selection.gameObjects.Length == 0) return;

		Transform transform = Selection.gameObjects[0].transform;
		for (int i = 0; i < transform.childCount; i++) 
		{
			ItemTransformData data;
			data = new ItemTransformData (transform.GetChild (i).name, transform.GetChild (i));
			asset.dataList.Add (data);
		}

		AssetDatabase.CreateAsset (asset, "Assets/MRBook/Resources/Data/" + transform.name +".asset");
		AssetDatabase.SaveAssets ();
	}
}
