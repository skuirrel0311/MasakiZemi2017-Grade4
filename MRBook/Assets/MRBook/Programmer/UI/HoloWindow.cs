using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloWindow : MonoBehaviour
{
    Vector3 zeroVec = Vector3.zero;
    Vector3 defaultScale;

    [SerializeField]
    TextMesh title = null;
    [SerializeField]
    TextMesh message = null;
    [SerializeField]
    HoloButton closeButton = null;

    [SerializeField]
    float moveSpeed = 4.0f;

    //maxRangeを超えたらminRangeになるまで動き続ける
    [SerializeField]
    float maxRange = 30.0f;
    [SerializeField]
    float minRange = 5.0f;

    //フェードアウト用
    Dictionary<IHoloUI, Color> colorDictionary = new Dictionary<IHoloUI, Color>();

    Color clearColor = Color.clear;

    Transform cameraTrans;

    bool isMovable = false;
    GameObject rootObj;

    MyCoroutine viewCoroutine;

    protected void Start()
    {
        defaultScale = transform.localScale;

        cameraTrans = Camera.main.transform;
        HoloText[] texts = GetComponentsInChildren<HoloText>();
        HoloSprite[] sprites = GetComponentsInChildren<HoloSprite>();

        for (int i = 0; i < texts.Length; i++)
        {
            colorDictionary.Add(texts[i], texts[i].Color);
        }
        for (int i = 0; i < sprites.Length; i++)
        {
            colorDictionary.Add(sprites[i], sprites[i].Color);
        }

        rootObj = transform.GetChild(0).gameObject;
    }

    public void Show(string title, string message, bool autoHide = false, float limitTime = 1.0f)
    {
        if (rootObj.activeSelf && viewCoroutine != null)
        {
            StopCoroutine(viewCoroutine);
        }

        //どんな状態で止められるかわからないのでここですべての初期化をしておく
        rootObj.SetActive(true);
        this.title.text = title;
        this.message.text = message;
        foreach (var c in colorDictionary)
        {
            c.Key.Color = c.Value;
        }
        transform.localScale = zeroVec;
        
        //出てくるときのアニメーション
        viewCoroutine = KKUtilities.FloatLerp(0.15f, (t) =>
        {
            transform.localScale = Vector3.Lerp(zeroVec, defaultScale, t * t);
        });

        //自動で消えない場合はCloseボタンが使えるようにする
        if (!autoHide)
            viewCoroutine.OnCompleted(() => closeButton.Refresh());
        else
            viewCoroutine.OnCompleted(() => KKUtilities.Delay(limitTime, () => Close(), this));

        StartCoroutine(viewCoroutine);
    }

    void Update()
    {
        if (cameraTrans == null)
            cameraTrans = Camera.main.transform;
        transform.position = cameraTrans.position;


        if (!rootObj.activeSelf) return;

        float angle = cameraTrans.eulerAngles.y - transform.eulerAngles.y;
        float temp = Mathf.Abs(angle);

        if (temp > maxRange) isMovable = true;
        if (temp < minRange) isMovable = false;

        if (!isMovable) return;

        //反対回り
        if (temp > 180.0f)
        {
            if (cameraTrans.eulerAngles.y > transform.eulerAngles.y)
                angle = -((360.0f - cameraTrans.eulerAngles.y) + transform.eulerAngles.y);
            else
                angle = (360.0f - transform.eulerAngles.y) + cameraTrans.eulerAngles.y;
        }

        Vector3 tempVec = transform.eulerAngles;
        tempVec.y += angle * Time.deltaTime * moveSpeed;
        transform.rotation = Quaternion.Euler(tempVec);
    }

    public void Close()
    {
        AkSoundEngine.PostEvent("Close", gameObject);

        float temp = 0.0f;
        viewCoroutine = KKUtilities.FloatLerp(0.1f, (t) =>
        {
            temp = t * t;
            foreach (var c in colorDictionary)
            {
                c.Key.Color = Color.Lerp(c.Value, clearColor, temp);
            }
        }).OnCompleted(() => rootObj.SetActive(false));

        StartCoroutine(viewCoroutine);
    }
}
