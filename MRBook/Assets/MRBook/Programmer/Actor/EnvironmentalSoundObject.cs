using UnityEngine;

public class EnvironmentalSoundObject : MonoBehaviour
{
    [SerializeField]
    string triggerName = "";

    void Start()
    {
        //MainSceneManager.I.OnPlayPage +=  PlaySound;
        
        //MainSceneManager.I.OnPageChanged += (arg1, arg2) =>
        //{
        //    MainSceneManager.I.OnPlayPage -=  PlaySound;
        //};
    }

    void PlaySound(BasePage page)
    {
        //AkSoundEngine.PostEvent(triggerName, gameObject);
    }
}
