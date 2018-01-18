using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KKUtilities;

/// <summary>
/// 3ページ目の食べ物を渡すところ
/// </summary>
public class PassItemInPage3 : BaseStateMachineBehaviour
{
    string otohimeHasFoodName;

    protected override void OnStart()
    {
        base.OnStart();
        StateMachineManager.I.StartCoroutine(TakeFood());
    }

    protected override BehaviourStatus OnUpdate()
    {
        return BehaviourStatus.Running;
    }

    IEnumerator TakeFood()
    {
        ActorManager actorManager = ActorManager.I;
        HoloCharacter urashima = actorManager.GetCharacter(ActorName.Urashima);
        HoloCharacter otohime = actorManager.GetCharacter(ActorName.Otohime);
        HoloItem item = ((CharacterItemSaucer)otohime.ItemSaucer).RightHandItem;


        CharacterItemSaucer urashimaItemSaucer = (CharacterItemSaucer)urashima.ItemSaucer;
        HoloItem urashimaItem = ((CharacterItemSaucer)urashima.ItemSaucer).RightHandItem;

        bool withEat = false;

        if (urashimaItem != null)
        {
            if (urashimaItem.name == "Fishingrod")
            {
                urashimaItemSaucer.DumpItem(false);
                actorManager.SetEnableObject(urashimaItem.name, false);
            }

            if (urashimaItem.GetName() == "Food")
            {
                //一緒に食べる
                withEat = true;
            }
        }

        if (withEat)
        {
            otohime.ChangeAnimationClip(MotionName.Eat, 0.1f);
            //一緒に食べるが、浦島は死ぬ
            urashima.ChangeAnimationClip(MotionName.Eat_Suffocation, 0.1f);
        }
        else
        {
            otohime.ChangeAnimationClip(MotionName.SitDelivery, 0.1f);
            urashima.ChangeAnimationClip(MotionName.TakeFood, 0.1f);
        }

        yield return new WaitForSeconds(5.0f);

        otohime.ItemSaucer.DumpItem(false);
        urashimaItemSaucer.SetItem(item, false, false);

        yield return new WaitForSeconds(5.0f);

        //食べ終わった
        urashima.ItemSaucer.DumpItem(false);
        actorManager.SetEnableObject(item.name, false);

        Debug.Log("on completed");
        CurrentStatus = BehaviourStatus.Success;
        OnEnd();
    }
}
