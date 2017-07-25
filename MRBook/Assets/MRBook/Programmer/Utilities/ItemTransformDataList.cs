using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTransformDataList : ScriptableObject
{
    [System.NonSerialized]
    public Transform parent = null;
	public List<ItemTransformData> dataList = new List<ItemTransformData>();
}

[System.Serializable]
public class ItemTransformData
{
	public string itemName;
	public Vector3 position;
	public Quaternion rotation;

	public ItemTransformData(string itemName, Transform transform)
	{
		this.itemName = itemName;
		position = transform.localPosition;
		rotation = transform.localRotation;
	}
}
