using UnityEngine;

public class EnvironmentalSoundObject : MonoBehaviour
{
    [SerializeField]
    string triggerName = "";

    void Start()
    {
        MainSceneManager.I.OnPlayPage += (arg1) => PlaySound();

        MainSceneManager.I.OnPlayEnd += (isSuccess) =>
        {
            MainSceneManager.I.OnPlayPage -= (arg1) => PlaySound();
        };
    }

    void PlaySound()
    {
        AkSoundEngine.PostEvent(triggerName, gameObject);
    }
}
