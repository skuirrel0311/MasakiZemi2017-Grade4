﻿using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;

public class AnchorPositionController : MyObjPositionController, IInputClickHandler
{
    Color startColor = Color.red;
    //動かせるか？
    public bool IsMovable { get; private set; }

    Renderer m_rendere;

    MyWorldAnchorManager anchorSroreManager;
    WorldAnchorStore anchorStore;

    [SerializeField]
    GameObject[] worldAnchors = null;
    //ゲームが開始された時の位置
    Vector3[] startWorldAnchorPositionArray;

    float updateIntervalTime = 5.0f;
    float time = 0.0f;

    protected override void Start()
    {
        base.Start();
        m_rendere = GetComponent<Renderer>();

        anchorSroreManager = MyWorldAnchorManager.I;

        anchorStore = null;

        startWorldAnchorPositionArray = new Vector3[worldAnchors.Length];

        MainSceneManager.I.OnGameStart += () =>
        {
            for (int i = 0; i < worldAnchors.Length; i++)
            {
                startWorldAnchorPositionArray[i] = worldAnchors[i].transform.position;
                Debug.Log("start world anchor position = " + startWorldAnchorPositionArray[i]);
            }
        };

        Debug.Log("start coroutine");
        StartCoroutine(anchorSroreManager.GetAnchorStore((anchorStore) =>
        {
            this.anchorStore = anchorStore;
            StartUpAnchor();
        }));
    }

    protected override void Update()
    {
        time += Time.deltaTime;

        if (time > updateIntervalTime)
        {
            time = 0.0f;

            Debug.Log("world anchor position = (" + worldAnchors[0].transform.position.x + "," + worldAnchors[0].transform.position.y + "," + worldAnchors[0].transform.position.z + ")");
            if (IsChangedAnchorPosition())
            {
                //設定しなおす
                Debug.Log("reload hologram");
                NotificationManager.I.ShowDialog("警告", "ホログラムのズレを検出しました。");

                DeleteAnchor();

                for (int i = 0; i < worldAnchors.Length; i++)
                {
                    worldAnchors[i].transform.position = startWorldAnchorPositionArray[i];
                }

                SaveAnchor();
            }
        }


        base.Update();
    }

    void StartUpAnchor()
    {
        WorldAnchor attached = anchorStore.Load(worldAnchors[0].name, worldAnchors[0]);

        if (attached == null)
        {
            SaveAnchor();
            m_rendere.material.color = Color.red;
        }
        else
        {
            //ここのコメントアウトを切れば初回のみアンカーが動かせる状態にできる
            //gameObject.SetActive(false);
            transform.position = worldAnchors[0].transform.GetChild(0).position;
            transform.rotation = worldAnchors[0].transform.GetChild(0).rotation;

            m_rendere.material.color = Color.blue;
        }

        startColor = m_rendere.material.color;
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (anchorStore == null) return;

        IsMovable = !IsMovable;

        if (IsMovable)
        {
            DeleteAnchor();
            m_rendere.material.color = Color.green;
        }
        else
        {
            SaveAnchor();
            m_rendere.material.color = startColor;
        }
    }

    public void Hide()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = false;
        }

        Collider[] cols = GetComponentsInChildren<Collider>();
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = false;
        }
    }

    public void SaveAnchor()
    {
        for (int i = 0; i < worldAnchors.Length; i++)
        {
            anchorSroreManager.SaveAnchor(worldAnchors[i]);
        }
    }

    public void DeleteAnchor()
    {
        for (int i = 0; i < worldAnchors.Length; i++)
        {
            anchorSroreManager.anchorStore.Delete(worldAnchors[i].name);
            DestroyImmediate(worldAnchors[i].GetComponent<WorldAnchor>());
        }
    }

    protected override void StartDragging()
    {
        if (!IsMovable) return;
        base.StartDragging();
    }

    bool IsChangedAnchorPosition()
    {
        for (int i = 0; i < worldAnchors.Length; i++)
        {
            if (!startWorldAnchorPositionArray[i].Equals(worldAnchors[i].transform.position)) return true;
        }
        return false;
    }
}
