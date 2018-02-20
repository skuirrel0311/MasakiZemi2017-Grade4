using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using KKUtilities;

public class StartPoint : EventAreaItemSaucer
{
    [SerializeField]
    UnityEvent onGameStart = null;
    [SerializeField]
    MotionName motionName = MotionName.Wait;

    [SerializeField]
    string mainCharacterName = "Urashima";

    GameObject circle;

    void Start()
    {
        circle = transform.GetChild(0).gameObject;

        MainSceneManager.I.OnPageLoaded += OnLoaded;
    }

    void OnLoaded(BasePage page)
    {
        circle.SetActive(true);

        MainSceneManager.I.OnPageLoaded -= OnLoaded;
    }

    public override void SetCharacter(HoloCharacter character, bool showParticle = true)
    {
        if(character.GetName() == mainCharacterName)
        {
            GameStart(character);
            return;
        }

        base.SetCharacter(character, showParticle);
    }

    void GameStart(HoloCharacter character)
    {
        if (onGameStart != null) onGameStart.Invoke();

        transform.GetChild(0).gameObject.SetActive(false);

        character.ChangeAnimationClip(motionName, 0.0f);
        character.transform.position = transform.position;
        character.transform.rotation = transform.rotation;
        
        AkSoundEngine.PostEvent("Equip", gameObject);
        ParticleManager.I.Play("Doron", owner.transform.position, Quaternion.identity);

        Utilities.Delay(0.2f, ()=> MainSceneManager.I.Play(), this);
    }

    protected override bool CheckCanHaveObject(HoloObject obj)
    {
        if (obj.GetName() == mainCharacterName) return true;
        return base.CheckCanHaveObject(obj);
    }

    public override void OnReset()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        base.OnReset();
    }
}
