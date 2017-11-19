using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;

public class HoloPuppet : HoloCharacter
{
    [SerializeField]
    BehaviourPuppet m_behavirour = null;

    [SerializeField]
    GameObject root = null;

    [SerializeField]
    PuppetMaster m_puppet = null;

    public Behaviour behaviour { get { return m_behavirour; } }
    public PuppetMaster puppet { get { return m_puppet; } }

    public override void ResetTransform()
    {
        BasePage page = transform.root.gameObject.GetComponent<BasePage>();
        page.StartCoroutine(ResetAction());
    }
    
    //現段階には問題があるかもしれないのでバグが出たら要チェック
    IEnumerator ResetAction()
    {
        DumpItem(HoloItem.Hand.Both);
        if (isMovable)
        {
            triangle.SetActive(true);
        }
        //ほかのページに持っていけるオブジェクトの場合はグローバルになっている可能性がある
        if (isBring)
        {
            ActorManager.I.RemoveGlobal(gameObject.name);
        }
        root.SetActive(false);
        yield return null;

        m_puppet.state = PuppetMaster.State.Dead;
        m_agent.enabled = false;
        yield return null;

        transform.position = firstPosition;
        transform.rotation = firstRotation;
        yield return null;
        
        m_puppet.state = PuppetMaster.State.Alive;
        root.SetActive(true);
        gameObject.SetActive(true);
        m_agent.enabled = true;
        //ページが開始された時のモーションに戻す
        m_puppet.pinWeight = 1.0f;
        m_animator.CrossFade(MotionNameManager.GetMotionName(firstAnimationName, this), 0.0f);
    }
}
