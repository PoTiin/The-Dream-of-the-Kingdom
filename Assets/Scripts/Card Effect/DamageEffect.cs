using Unity.Mathematics;
using UnityEngine;
[CreateAssetMenu(fileName = "DamageEffect", menuName = "Card Effect/DamageEffect")]
public class DamageEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (target == null) return;
        var damage = (int)math.round(value * from.baseStrength);
        switch (targetType)
        {
            case EffectTargetType.Self:
                break;
            case EffectTargetType.Target:
                
                target.TakeDamage(damage);
                Debug.Log($"÷¥––¡À{damage}µ„…À∫¶!");
                break;
            case EffectTargetType.All:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().TakeDamage(damage);
                }
                break;
            default:
                break;
        }
    }
}
