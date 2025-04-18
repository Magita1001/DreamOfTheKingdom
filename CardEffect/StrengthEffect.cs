using UnityEngine;

[CreateAssetMenu(fileName = "StrengthEffect", menuName = "Card Effect/StrengthEffect")]
public class StrengthEffect : Effect
{
    public override void Execute(CharacterBase form, CharacterBase target)
    {
        switch (targetType)
        {
            case EffectTargetType.Self:
                form.SetupStrength(value, true);
                break;
            case EffectTargetType.Target:
                target.SetupStrength(value, false);
                //target.UpdateStrengthRound();
                break;
            case EffectTargetType.ALL:
                break;
        }
    }
}
