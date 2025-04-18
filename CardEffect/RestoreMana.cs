using UnityEngine;

[CreateAssetMenu(fileName = "RestoreMana", menuName = "Card Effect/RestoreMana")]
public class RestoreMana : Effect
{
    public override void Execute(CharacterBase form, CharacterBase target)
    {
        if (form is Player player)
        {
            form.BuffAnimation();
            player = (Player)form;
            player.CurrentMana += value;

            player.UpdateMana(0);
        }
    }
}
