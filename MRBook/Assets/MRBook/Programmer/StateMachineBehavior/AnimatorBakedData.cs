using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorBakedData : ScriptableObject
{
    int totalStateNum = 0;

    //Dictionaryがクズなので
    public List<int> hashList = new List<int>();
    public List<MyAnimatorState> stateList = new List<MyAnimatorState>();

    [System.Serializable]
    public class MyAnimatorState
    {
        public int index;
        public int length;
        public int nameHash;

        public MyAnimatorState(int nameHash, int index, int length)
        {
            this.nameHash = nameHash;
            this.index = index;
            this.length = length;
        }
    }

    public void AddDictionary(int nameHash,  int length)
    {
        MyAnimatorState state = new MyAnimatorState(nameHash, totalStateNum, length);
        hashList.Add(nameHash);
        stateList.Add(state);
        totalStateNum += length;
    }
}