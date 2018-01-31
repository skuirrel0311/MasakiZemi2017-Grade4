using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class MissionTextController : BaseManager<MissionTextController>
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

    protected override void Start()
    {
        base.Start();
        MainSceneManager sceneManager = MainSceneManager.I;
        mainCameraTransform = Camera.main.transform;

        sceneManager.OnPageInitialized += (page) =>
        {
            endTexts[(int)Mode.Track].gameObject.SetActive(false);
            ChangeMode(Mode.Track);

            string firstText = page.storyFirstText;
            string endText = page.storyEndText;

            SetText(firstText, endText);
            ResetText();

            Utilities.Delay(1.0f, () =>
            {
                endTexts[(int)Mode.Track].gameObject.SetActive(true);
            }, this);


            Utilities.Delay(5.0f, () =>
            {
                ChangeMode(Mode.Statics);
            }, this);
        };

        sceneManager.OnPlayPage += () =>
        {
            endTexts[1].gameObject.SetActive(false);
        };

        sceneManager.OnReset += () =>
        {
            ResetText();
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
        MainSceneManager.I.PageLoaded();
    }

    void SetText(string first, string end)
    {
        first = SetNewLine(first);
        end = SetNewLine(end);

        for (int i = 0; i < modeNum; i++)
        {
            firstTexts[i].CurrentText = first;
            endTexts[i].CurrentText = end;
        }
    }

    void ResetText()
    {
        endTexts[1].gameObject.SetActive(true);
        storyText.CurrentText = "";
    }

    public void AddText(string text)
    {
        text = SetNewLine(text);

        if (string.IsNullOrEmpty(storyText.CurrentText))
        {
            storyText.CurrentText = text;
        }
        else
        {
            storyText.CurrentText = storyText.CurrentText + '\n' + text;
        }
    }

    string SetNewLine(string text)
    {
        string[] texts = text.Split(',');

        text = texts[0];

        for (int i = 1; i < texts.Length; i++)
        {
            text += '\n' + texts[i];
        }

        return text;
    }
}
