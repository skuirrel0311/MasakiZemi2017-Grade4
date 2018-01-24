using System.Collections;
using UnityEngine;
using KKUtilities;

/// <summary>
/// タイトルで設定された時の位置からずれた時に自動で直してくれる人
/// </summary>
public class BookPositionModifier : BaseManager<BookPositionModifier>
{
    public Transform bookTransform = null;
    Vector3 pagePosition;

    //部屋に配置するWorldAnchor
    [SerializeField]
    WorldAnchorController worldAnchor = null;

    [SerializeField]
    float positionCheckInterval = 1.0f;

    Vector3 oldWorldAnchorPosition;

    const string saveBookPositionXKey = "BookPositionX";
    const string saveBookPositionYKey = "BookPositionY";
    const string saveBookPositionZKey = "BookPositionZ";
    const string saveBookRotationXKey = "BookRotationX";
    const string saveBookRotationYKey = "BookRotationY";
    const string saveBookRotationZKey = "BookRotationZ";

    protected override void Start()
    {
#if !UNITY_EDITOR
        StartCoroutine(MonitorWorldAnchor());
        oldWorldAnchorPosition = worldAnchor.transform.position;
#endif
        pagePosition = bookTransform.position;
        base.Start();
    }

    protected override void OnDestroy()
    {
        SaveBookLocation();
        base.OnDestroy();
    }

    public void LoadBookLocation()
    {
        Utilities.Delay(2, () =>
        {
            Debug.Log("on book load");
            Vector3 vec;
            vec.x = PlayerPrefs.GetFloat(saveBookPositionXKey);
            vec.y = PlayerPrefs.GetFloat(saveBookPositionYKey);
            vec.z = PlayerPrefs.GetFloat(saveBookPositionZKey);
            bookTransform.localPosition = vec;

            vec.x = PlayerPrefs.GetFloat(saveBookRotationXKey);
            vec.y = PlayerPrefs.GetFloat(saveBookRotationYKey);
            vec.z = PlayerPrefs.GetFloat(saveBookRotationZKey);
            bookTransform.localEulerAngles = vec;
            TitleView.I.transform.position = bookTransform.position;
            TitleView.I.transform.rotation = bookTransform.rotation;
        }, this);

    }

    public void SaveBookLocation()
    {
        Vector3 vec = bookTransform.localPosition;
        PlayerPrefs.SetFloat(saveBookPositionXKey, vec.x);
        PlayerPrefs.SetFloat(saveBookPositionYKey, vec.y);
        PlayerPrefs.SetFloat(saveBookPositionZKey, vec.z);

        vec = bookTransform.localEulerAngles;

        PlayerPrefs.SetFloat(saveBookRotationXKey, vec.x);
        PlayerPrefs.SetFloat(saveBookRotationYKey, vec.y);
        PlayerPrefs.SetFloat(saveBookRotationZKey, vec.z);
    }

    IEnumerator MonitorWorldAnchor()
    {
        WaitForSeconds wait = new WaitForSeconds(positionCheckInterval);

        while (true)
        {
            yield return wait;

            if (IsChangeWorldAnchorPosition())
            {
                ModifyBookPosition(false);
                Debug.Log("modify");
            }
            oldWorldAnchorPosition = worldAnchor.transform.position;
        }
    }

    bool IsChangeWorldAnchorPosition()
    {
        float temp = 0.3f;

        Vector3 difVec = oldWorldAnchorPosition - worldAnchor.transform.position;

        if (Mathf.Abs(difVec.x) > temp) return true;
        if (Mathf.Abs(difVec.y) > temp) return true;
        if (Mathf.Abs(difVec.z) > temp) return true;

        return false;
    }

    public void ModifyBookPosition(bool showDialog)
    {
        Vector3 movement = bookTransform.position - pagePosition;
        pagePosition = bookTransform.position;

        MainSceneManager.I.SetBookPositionOffset(movement);
        if (showDialog)
        {
            AkSoundEngine.PostEvent("Alart", gameObject);
            NotificationManager.I.ShowDialog("警告", "ホログラムのずれを検知しました。", true, 3.0f);
        }
    }

    public void SetWorldAnchorsRendererActive(bool isActive)
    {
        //gameObjectのSetActiveではない
        worldAnchor.SetActive(isActive);
    }

    public void WorldAnchorsOperation(bool isSave)
    {
        if (isSave)
            worldAnchor.SaveAnchor();
        else
            worldAnchor.DeleteAnchor();
    }
}
