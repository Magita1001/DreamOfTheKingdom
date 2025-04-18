using UnityEngine;

public class Player : CharacterBase
{
    //Variable文件的作用不是用来设置数值 而是读取,记录数值并传递出去
    public IntVariable playerMana;
    public int maxMana;
    public int CurrentMana { get => playerMana.currentValue; set => playerMana.SetValue(value); }

    private void OnEnable()
    {
        playerMana.maxValue = maxMana;
        CurrentMana = playerMana.maxValue; //设置初始法力值
    }

    /// <summary>
    /// 每回合开始时做的事情 在监听函数中添加
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
