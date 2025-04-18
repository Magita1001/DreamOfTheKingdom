using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Card Effect/DrawCardEffect")]
public class DrawCardEffect : Effect
{
    public IntEventSO drawCardEvent;
    public override void Execute(CharacterBase form, CharacterBase target)
    {
        //实现抽卡效果
        drawCardEvent?.RaisEvent(value, this);
    }
}
