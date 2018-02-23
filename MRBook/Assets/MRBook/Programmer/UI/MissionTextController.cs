using System.Collections;
using UnityEngine;
using KKUtilities;

public class MissionTextController : BaseManager<MissionTextController>
{
    public enum Mode { Track, Statics }

    Mode currentMode;
    const int modeNum = 2;
    Transform mainCameraTransform;

    [SerializeField]
    float trackTime = 5.0f;

    [SerializeField]
    BodyLocked bodyLoacked = null;

    [SerializeField]
    GameObject[] storyTextWindows = null;

    [SerializeField]
    Transform staticWindowLocation = null;

    [SerializeField]
    HoloText[] missionTexts = null;

    [SerializeField]
    HoloText firstText = null;
    [SerializeField]
    HoloText endText = null;
    [SerializeField]
    HoloText storyText = null;
    [SerializeField]
    HoloText hintText = null;

    protected override void Start()
    {
        base.Start();
        MainSceneManager sceneManager = MainSceneManager.I;
        mainCameraTransform = Camera.main.transform;

        sceneManager.OnPageLoaded += (page) =>
        {
            ChangeMode(Mode.Track);

            missionTexts[(int)Mode.Statics].CurrentText = page.missionText;
            missionTexts[(int)Mode.Track].CurrentText = page.missionText;
            firstText.CurrentText = page.storyFirstText;
            endText.CurrentText = page.storyEndText;

            ResetText();
            
            Utilities.Delay(trackTime, () =>
            {
                ChangeMode(Mode.Statics);
            }, this);
        };

        sceneManager.OnPlayPage += () =>
        {
            endText.gameObject.SetActive(false);
            missionTexts[(int)Mode.Statics].gameObject.SetActive(false);
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
            storyTextWindows[(int)Mode.Statics].SetActive(false);
            storyTextWindows[(int)Mode.Track].SetActive(true);
            bodyLoacked.enabled = true;
        }
    }

    public void Disable()
    {
        storyTextWindows[0].SetActive(false);
        storyTextWindows[1].SetActive(false);
    }

    IEnumerator MoveWindow()
    {
        Vector3 firstPosition = storyTextWindows[(int)Mode.Track].transform.position;
        Quaternion firstRotation = storyTextWindows[(int)Mode.Track].transform.rotation;
        Vector3 endPosition = staticWindowLocation.position;
        Quaternion endRotation = staticWindowLocation.rotation;

        bodyLoacked.enabled = false;

        yield return StartCoroutine(Utilities.FloatLerp(1.0f, (t) =>
        {
            storyTextWindows[(int)Mode.Track].transform.SetPositionAndRotation(
                Vector3.Lerp(firstPosition, endPosition, t),
                Quaternion.Lerp(firstRotation, endRotation, t)
            );
        }));

        //todo:きちんとしたアニメーションを導入する
        storyTextWindows[(int)Mode.Track].SetActive(false);
        storyTextWindows[(int)Mode.Statics].SetActive(true);
    }

    void ResetText()
    {
        hintText.gameObject.SetActive(false);
        endText.gameObject.SetActive(true);
        missionTexts[(int)Mode.Statics].gameObject.SetActive(true);
        storyText.CurrentText = "";
    }

    public void AddText(string text)
    {
        if (string.IsNullOrEmpty(storyText.CurrentText))
        {
            storyText.CurrentText = text;
        }
        else
        {
            storyText.CurrentText = storyText.CurrentText + '\n' + text;
        }
    }

    public void SetHint(string text)
    {
        hintText.gameObject.SetActive(true);
        hintText.CurrentText = text;
    }
}
