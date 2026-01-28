using UnityEngine;

public class Enemy : CharacterBase
{
    public EnemyActionDataSO actionDataSO;
    public EnemyAction currentAction;
    protected Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public virtual void OnPlayerTurnBegin()
    {
        var randomIndex = Random.Range(0, actionDataSO.actions.Count);
        currentAction = actionDataSO.actions[randomIndex];
    }

    public virtual void OnEnemyTurnBegin()
    {
        switch (currentAction.effect.targetType)
        {
            case EffectTargetType.Self:
                Skill();
                break;
            case EffectTargetType.Target:
                Attack();
                break;
            case EffectTargetType.All:
                break;
            default:
                break;
        }
    }

    public virtual void Skill()
    {
        currentAction.effect.Execute(this, this);
    }

    public virtual void Attack()
    {
        currentAction.effect.Execute(this, player);
    }
}
