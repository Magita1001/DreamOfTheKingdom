using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Card Effect/DamageEffect")]
public class DamageEffect : Effect
{
    public override void Execute(CharacterBase form, CharacterBase target)
    {
        if (target == null) return;

        switch (targetType)
        {
            case EffectTargetType.Target:
                var damage = (int)math.round(value * form.baseStrength);
                target.TakeDamage(damage);
                Debug.Log($"ִ����{damage}���˺�");

                break;
            case EffectTargetType.ALL:
                foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().TakeDamage(value);
                }
                break;

        }
    }
}
