using UnityEngine;

public class MainGameManager : BaseManager<MainGameManager>
{
    Animator m_Animator;

    protected override void Start()
    {
        base.Start();
        m_Animator = GetComponent<Animator>();
    }

    public void GameStart()
    {
        //イベントのトリガーをチェックしていく
        GameObject[] eventTriggers = GameObject.FindGameObjectsWithTag("Trigger");

        for(int i = 0;i< eventTriggers.Length;i++)
        {
            eventTriggers[i].GetComponent<MyEventTrigger>().SetFlag();
        }

        m_Animator.SetBool("IsStart", true);
    }
}
