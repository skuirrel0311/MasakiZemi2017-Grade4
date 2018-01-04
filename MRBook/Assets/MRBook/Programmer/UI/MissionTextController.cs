using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class MissionTextController : MonoBehaviour
{
    public enum Mode { Track, Statics }

    Mode currentMode;
    const int modeNum = 2;
    Transform mainCameraTransform;
    
    [SerializeField]
    BodyLocked bodyLoacked = null;

    [SerializeField]
    GameObject[] missionTexts = null;

    [SerializeField]
    HoloText[] firstTexts = null;
    [SerializeField]
    HoloText[] endTexts = null;

    [SerializeField]
    HoloText storyText = null;

    void Start()
    {
        MainSceneManager sceneManager = MainSceneManager.I;
        mainCameraTransform = Camera.main.transform;

        sceneManager.OnPageLoaded += () =>
        {
            ChangeMode(Mode.Track);

            string firstText = sceneManager.pages[sceneManager.currentPageIndex].storyFirstText;
            string endText = sceneManager.pages[sceneManager.currentPageIndex].storyEndText;

            SetText(firstText, endText);

            Utilities.Delay(3.0f, () =>
            {
                ChangeMode(Mode.Statics);
            }, this);
        };
    }

    public void ChangeMode(Mode mode)
    {
        currentMode = mode;

        if (mode == Mode.Statics)
        {
            StartCoroutine(MoveWindow());
        }
        else if (mode == Mode.Track)
        {
            missionTexts[1].SetActive(false);
            missionTexts[0].SetActive(true);
            bodyLoacked.enabled = true;
        }
    }

    IEnumerator MoveWindow()
    {
        Vector3 firstPosition = missionTexts[0].transform.position;
        Quaternion firstRotation = missionTexts[0].transform.rotation;
        Vector3 endPosition = missionTexts[1].transform.position;
        Quaternion endRotation = missionTexts[1].transform.rotation;

        bodyLoacked.enabled = false;

        yield return StartCoroutine(Utilities.FloatLerp(1.0f, (t) =>
        {
            missionTexts[0].transform.SetPositionAndRotation(
                Vector3.Lerp(firstPosition, endPosition, t), 
                Quaternion.Lerp(firstRotation, endRotation, t)
            );
        }));

        missionTexts[0].SetActive(false);
        missionTexts[1].SetActive(true);
    }

    public void SetText(string first, string end)
    {
        for (int i = 0; i < modeNum; i++)
        {
            firstTexts[i].CurrentText = first;
            endTexts[i].CurrentText = end;
        }
    }

    public void ResetText()
    {
        storyText.CurrentText = "";
    }

    public void AddText(string text)
    {
        storyText.CurrentText = storyText.CurrentText + '\n' + text;
    }
}
