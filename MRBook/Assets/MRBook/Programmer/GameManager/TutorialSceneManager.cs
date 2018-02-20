using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

public class TutorialSceneManager : BaseManager<TutorialSceneManager>
{
    [SerializeField]
    HoloButton startButton = null;

    bool isStart = false;

    [SerializeField]
    HoloButton resetButton = null;
    
    [SerializeField]
    HoloPuppet urashima = null;

    [SerializeField]
    HoloObject book = null;

    [SerializeField]
    HoloObject[] objs = null;

    [SerializeField]
    Transform uiContainer = null;

    [SerializeField]
    LayerMask layerMask;

    HoloObjResetManager resetManager;

    public Action onReset;

    Vector3 hitPoint;

    bool hitWater = false;

    [SerializeField]
    GameObject tutorial = null;
    [SerializeField]
    GameObject tutorialPosition = null;

    protected override void Start()
    {
        base.Start();
        resetManager = new HoloObjResetManager(this);

        urashima.Init();
        resetManager.AddResetter(urashima.Resetter);
        book.Init();
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].Init();
            resetManager.AddResetter(objs[i].Resetter);
        }
    }

    public void SetTutorial()
    {
        tutorialPosition.SetActive(false);
        tutorial.transform.SetPositionAndRotation(tutorialPosition.transform.position, tutorialPosition.transform.rotation);
        uiContainer.SetPositionAndRotation(tutorialPosition.transform.position, tutorialPosition.transform.rotation);
        Utilities.Delay(10, () =>
        {
            tutorial.SetActive(true);
            if (!isStart) startButton.gameObject.SetActive(true);
            else resetButton.gameObject.SetActive(true);
        }, this);
    }

    public void ActiveDebugMode()
    {
        if (!isStart) startButton.gameObject.SetActive(false);
        else resetButton.gameObject.SetActive(false);
        tutorial.SetActive(false);
        tutorialPosition.SetActive(true);
    }

    public void StartTutorial()
    {
        isStart = true;
        urashima.RootObject.SetActive(true);
        resetButton.gameObject.SetActive(true);
    }

    public void DeadUrashima()
    {
        HoloObjectController.I.Disable();

        Ray ray = new Ray(urashima.transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3, layerMask, QueryTriggerInteraction.Collide))
        {
            hitPoint = hit.point;

            StartCoroutine(FallUrashima());
        }
    }

    IEnumerator FallUrashima()
    {
        Debug.Log("fall in scene manager");
        float bookHeight = OffsetController.I.bookTransform.position.y - 0.2f;
        bool enablePuppet = false;

        while(true)
        {
            urashima.transform.position += Vector3.down * Time.deltaTime;

            if(!enablePuppet && urashima.transform.position.y < bookHeight + 0.5f)
            {
                enablePuppet = true;
                urashima.Puppet.mode = RootMotion.Dynamics.PuppetMaster.Mode.Active;
                urashima.Puppet.state = RootMotion.Dynamics.PuppetMaster.State.Dead;
            }

            if(urashima.transform.position.y < bookHeight)
            {
                break;
            }

            yield return null;
        }

        urashima.RootObject.SetActive(false);

        yield return new WaitForSeconds(1.0f);

        ShowUrashimaSaul(hitPoint);
    }

    void ShowUrashimaSaul(Vector3 point)
    {
        if (hitWater)
        {
            ParticleManager.I.Play("UrashimaSoul", point);
        }
        else
        {
            ParticleManager.I.Play("UrashimaSoul", urashima.transform.position);
        }
        AkSoundEngine.PostEvent("Die", gameObject);
    }

    public void HitWater()
    {
        hitWater = true;
        ParticleManager.I.Play("Splash", hitPoint);
        AkSoundEngine.PostEvent("Splash", gameObject);
    }

    public void OnReset()
    {
        if (onReset != null) onReset.Invoke();
        hitWater = false;
        resetManager.Reset();

        Utilities.Delay(10, () => resetButton.Refresh(), this);
    }
}
