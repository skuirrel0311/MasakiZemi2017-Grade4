using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTransformDataList : ScriptableObject
{
    public HoloItem.Hand hand;
	public List<ItemTransformData> dataList = new List<ItemTransformData>();
}

[System.Serializable]
public class ItemTransformData
{
	public string itemName;
	public Vector3 position;
	public Vector3 rotation;
    public MotionName motionName;
    public bool canTake = true;

	public ItemTransformData(string itemName, Transform transform)
	{
		this.itemName = itemName;
		position = transform.localPosition;
		rotation = transform.localEulerAngles;
	}
}
