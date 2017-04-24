using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenButton : MonoBehaviour
{
    public bool IsPushing { get; private set; }
    public bool IsJustPush { get; private set; }

    public float pushingTime { get; private set; }

    void Start()
    {
        IsPushing = false;
        IsJustPush = false;

        EventTrigger trigger = GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((n) =>
        {
            IsJustPush = true;
            IsPushing = true;
        });
        trigger.triggers.Add(entry);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerUp;
        entry2.callback.AddListener((n) =>
        {
            IsPushing = false;
        });
        trigger.triggers.Add(entry2);
    }

    void Update()
    {
        if (IsPushing) pushingTime += Time.deltaTime;
        else pushingTime = 0.0f;
    }

    void LateUpdate()
    {
        IsJustPush = false;
    }
}
