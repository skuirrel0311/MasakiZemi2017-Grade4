using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : BaseManager<NotificationManager>
{
    [SerializeField]
    Image m_Image;
    Text m_Text;

    Coroutine popupCoroutine = null;
    bool isPopUp = false;

    protected override void Start()
    {
        m_Text = m_Image.GetComponentInChildren<Text>();
        base.Start();
    }

    public void PopUpMessage(string message, int fontSize = 30, float PopupDuration = 1.0f)
    {
        if (isPopUp)
        {
            StopCoroutine(popupCoroutine);
            Debug.Log("中断された");
        }
        m_Text.fontSize = fontSize;
        m_Text.text = message;
        isPopUp = true;

        popupCoroutine = StartCoroutine(ShowMessage(PopupDuration));
    }

    IEnumerator ShowMessage(float PopupDuration)
    {
        Coroutine lerpCoroutine;
        Color whiteColor = Color.white;
        Color clearColor = Color.clear;
        Color currentColor;

        m_Image.color = clearColor;
        m_Text.color = clearColor;
        m_Image.gameObject.SetActive(true);

        lerpCoroutine = StartCoroutine(KKUtilities.FloatLerp(1.0f, (n) =>
        {
            m_Image.color = Color.Lerp(clearColor, whiteColor, n * n);
            m_Text.color = Color.Lerp(clearColor, Color.black, n * n);
        }));
        yield return lerpCoroutine;

        yield return new WaitForSeconds(PopupDuration);

        lerpCoroutine = StartCoroutine(KKUtilities.FloatLerp(1.0f, (n) =>
        {
            currentColor = whiteColor * Time.deltaTime;
            m_Image.color -= currentColor;
            m_Text.color -= currentColor;
        }));

        yield return lerpCoroutine;

        m_Image.gameObject.SetActive(false);
        isPopUp = false;
    }

    public void ClearMessage()
    {
        if (isPopUp)
        {
            StopCoroutine(popupCoroutine);
        }

        popupCoroutine = StartCoroutine(DestroyMessage());
    }

    IEnumerator DestroyMessage()
    {
        Color clearColor = Color.clear;
        Color textStartColor = m_Text.color;
        Color imageStartColor = m_Image.color;

        Coroutine lerpCoroutine;

        lerpCoroutine = StartCoroutine(KKUtilities.FloatLerp(0.5f, (n) =>
        {
            m_Image.color = Color.Lerp(imageStartColor, clearColor, n * n);
            m_Text.color = Color.Lerp(textStartColor, clearColor, n * n);
        }));

        yield return lerpCoroutine;
    }
}
