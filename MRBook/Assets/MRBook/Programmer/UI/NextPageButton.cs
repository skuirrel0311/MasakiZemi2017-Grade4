using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPageButton : MonoBehaviour
{
    [SerializeField]
    HoloButton button = null;

    [SerializeField]
    HoloText text = null;

    MainSceneManager sceneManager;

    void Start()
    {
        sceneManager = MainSceneManager.I;

        button.AddListener(() =>
        {
            int nextPageIndex = sceneManager.currentPageIndex + 1;
            if(nextPageIndex >= sceneManager.pages.Length)
            {
                return;
            }
            sceneManager.ChangePage(nextPageIndex);
        });

        sceneManager.OnPageLoaded += (page) =>
        {
            text.gameObject.SetActive(false);
            button.Hide();
        };
        sceneManager.OnPlayEnd += (success) =>
        {
            if (sceneManager.currentPageIndex == 4) return;

            if (success)
            {
                text.gameObject.SetActive(true);
                button.Refresh();
            }

        };
    }
}
