using UnityEngine;

[CreateAssetMenu(fileName = "DefenseEffect", menuName = "Card Effect/DefenseEffect")]
public class DefenseEffect : Effect
{
    public override void Execute(CharacterBase form, CharacterBase target)
    {
        if (targetType == EffectTargetType.Self)
        {
            form.BuffAnimation();
            form.UpdateDefense(value);
        }

        if (targetType == EffectTargetType.Target)
        {
            target.UpdateDefense(value);
        }
    }
}
