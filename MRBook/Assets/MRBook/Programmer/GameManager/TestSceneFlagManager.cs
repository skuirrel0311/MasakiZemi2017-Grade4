using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSceneFlagManager : FlagManager
{
    [SerializeField]
    Text textPrefab = null;

    [SerializeField]
    Transform textContainer = null;

    Dictionary<string, Text> flagTextDictionary = new Dictionary<string, Text>();

    protected override void Start()
    {
        base.Start();
        TestSceneManager.I.OnPageInitialized += (page) =>
        {
            foreach(string key in flagTextDictionary.Keys)
            {
                Destroy(flagTextDictionary[key]);
            }

            flagTextDictionary.Clear();
        };
    }

    public override void SetFlag(string name, MyEventTrigger eventTrigger, bool isFlagged = true)
    {
        base.SetFlag(name, eventTrigger, isFlagged);

        Text text;
        
        if(!flagTextDictionary.TryGetValue(name, out text))
        {
            text = Instantiate(textPrefab, textContainer);
            flagTextDictionary.Add(name, text);
        }

        text.text = name + " = " + isFlagged.ToString();
    }

}
