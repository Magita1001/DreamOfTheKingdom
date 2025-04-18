using UnityEngine;

public class Player : CharacterBase
{
    //Variable�ļ������ò�������������ֵ ���Ƕ�ȡ,��¼��ֵ�����ݳ�ȥ
    public IntVariable playerMana;
    public int maxMana;
    public int CurrentMana { get => playerMana.currentValue; set => playerMana.SetValue(value); }

    private void OnEnable()
    {
        playerMana.maxValue = maxMana;
        CurrentMana = playerMana.maxValue; //���ó�ʼ����ֵ
    }

    /// <summary>
    /// ÿ�غϿ�ʼʱ�������� �ڼ������������
    /// </summary>
    public void NewTurn()
    {
        CurrentMana = maxMana;
    }

    public void UpdateMana(int cost)
    {
        CurrentMana -= cost;
        if (CurrentMana <= 0)
        {
            CurrentMana = 0;
        }
    }

    public void NewGame()
    {
        CurrentHP = maxHp;
        isDead = false;
        buffRound.currentValue = buffRound.maxValue;
        NewTurn();
    }
}
