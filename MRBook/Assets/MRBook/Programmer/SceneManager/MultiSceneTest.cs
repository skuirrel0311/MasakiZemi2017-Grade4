using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiSceneTest : MonoBehaviour
{
    [SerializeField]
    GameObject[] prefabs = null;
    int index = 0;

    List<GameObject> objList = new List<GameObject>();

    public int objMaxNum = 10000;

    void Start()
    {
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            int temp = objMaxNum / 10;

            for (int i = 0; i < objList.Count; i++)
            {
                Destroy(objList[i]);
            }

            for (int i = 0;i < temp;i++)
            {
                GameObject obj = Instantiate(prefabs[index], new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f)), Quaternion.identity);
                objList.Add(obj);
                index++;
                if (index == prefabs.Length) index = 0;
            }


        }
    }
}
