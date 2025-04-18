using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Card Effect/DrawCardEffect")]
public class DrawCardEffect : Effect
{
    public IntEventSO drawCardEvent;
    public override void Execute(CharacterBase form, CharacterBase target)
    {
        //ʵ�ֳ鿨Ч��
        drawCardEvent?.RaisEvent(value, this);
    }
}
