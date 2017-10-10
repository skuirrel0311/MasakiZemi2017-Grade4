using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using UnityEditor;

public class GetCurrentSelectObject : Editor 
{
	[MenuItem("Mytools/CreateItemTransformDataAsset")]
	static void DrawText()
	{
		ItemTransformDataList asset = CreateInstance<ItemTransformDataList> ();

		if (Selection.gameObjects.Length == 0) return;

		Transform transform = Selection.gameObjects[0].transform;
		for (int i = 0; i < transform.childCount; i++) 
		{
			ItemTransformData data;
			data = new ItemTransformData (transform.GetChild (i).name, transform.GetChild (i));
			asset.dataList.Add (data);
		}

		//そもそも同じファイルが存在するのかをチェックする
		ItemTransformDataList original;
		string filePath = "Assets/MRBook/Resources/Data/" + transform.root.name +  transform.name +".asset";
        

		//同じファイルは存在しなかった
		if (!TryGetOriginalFile (transform.root.name + transform.name, out original)) 
		{
			AssetDatabase.CreateAsset (asset, filePath);
			AssetDatabase.SaveAssets ();
			return;
		}

		//同じファイルが見つかった
		if (EditorUtility.DisplayDialog ("override", transform.name + ".assetを上書きしますか？", "上書きする", "中止")) 
		{
            //データの差し替え
            AssetDatabase.DeleteAsset(filePath);
            AssetDatabase.CreateAsset(asset, filePath);
            AssetDatabase.SaveAssets();
		}
	}

	static bool TryGetOriginalFile(string fileName, out ItemTransformDataList result)
	{
		ItemTransformDataList original = Resources.Load<ItemTransformDataList> ("Data/" + fileName);
		result = original;
		return result == null ? false : true;
	}
}
