using System.Collections;
using UnityEngine;

public class Enemy : CharacterBase
{
    public EnemyActionListSO actionDataSO;

    public EnemyAction currentAction;

    protected Player player;

    protected override void Awake()
    {
        base.Awake();
    }

    public virtual void OnPlayerTurnBegin()
    {
        var randomIndex = Random.Range(0, actionDataSO.actions.Count);
        currentAction = actionDataSO.actions[randomIndex];
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public virtual void OnEnemyTurnBegin()
    {
        ResetDefense();
        switch (currentAction.effect.targetType)
        {
            case EffectTargetType.Self:
                Skill();
                break;
            case EffectTargetType.Target:
                Attack();
                break;
            case EffectTargetType.ALL:
                break;
        }
    }

    public virtual void Skill()
    {
        //animator.SetTrigger("skill");
        //currentAction.effect.Execute(this, this);
        StartCoroutine(ProcessDelayAction("skill"));
    }

    public virtual void Attack()
    {
        //animator.SetTrigger("attack");
        //currentAction.effect.Execute(this, player);
        StartCoroutine(ProcessDelayAction("attack"));
    }

    IEnumerator ProcessDelayAction(string actionName)
    {
        animator.SetTrigger(actionName);

        //normalizedTime % 1.0f 可以理解为去除左侧的一位数 1.65 % 1.0 = 0.65
        yield return new WaitUntil(() =>
              animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f > 0.55f
              && !animator.IsInTransition(0)
              && animator.GetCurrentAnimatorStateInfo(0).IsName(actionName)
        );

        if (actionName == "attack")
        {
            currentAction.effect.Execute(this, player);
        }
        else
        {
            currentAction.effect.Execute(this, this);
        }
    }

}
