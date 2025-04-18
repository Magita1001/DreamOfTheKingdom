using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int maxHp;
    public IntVariable hp;
    public IntVariable defense;
    public IntVariable buffRound;

    public int CurrentHP { get => hp.currentValue; set => hp.SetValue(value); }
    public int MaxHp { get => hp.maxValue; }

    public  Animator animator;
    public  bool isDead;

    public GameObject buff;
    public GameObject debuff;    

    //力量相关
    public float baseStrength = 1f;
    public  float strengthEffect = 0.5f;

    [Header("事件广播")]
    public ObjectEventSO characterDeadEvent;


    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        hp.maxValue = maxHp;
        CurrentHP = MaxHp;

        buffRound.currentValue = 0;

        ResetDefense();
    }

    protected virtual void Update()
    {
        animator.SetBool("isDead", isDead);
    }

    public virtual void TakeDamage(int damage)
    {
        var currentDamage = (damage - defense.currentValue) >= 0 ? (damage - defense.currentValue) : 0;
        var currentDefense = (damage - defense.currentValue) >= 0 ? 0 : (defense.currentValue - damage);
        defense.SetValue(currentDefense);

        if (CurrentHP > currentDamage)
        {
            CurrentHP -= currentDamage;
            animator.SetTrigger("hit");
        }
        else
        {
            CurrentHP = 0;
            //当前人物死亡
            isDead = true;
            characterDeadEvent?.RaisEvent(this, this);
        }
    }

    public void UpdateDefense(int amount)
    {
        var value = defense.currentValue + amount;
        defense.SetValue(value);
    }

    public void ResetDefense()
    {
        defense.SetValue(0);
    }

    public void HealHealth(int amount)
    {
        CurrentHP += amount;
        CurrentHP = Mathf.Min(CurrentHP, MaxHp);
        buff.SetActive(true);
    }


    public void SetupStrength(int round, bool isPostive)
    {
        if (isPostive)
        {
            float newStrength = baseStrength + strengthEffect;
            baseStrength=Mathf.Min(newStrength, 1.5f);
            buff.SetActive(true);
        }
        else
        {
            debuff.SetActive(true);
            baseStrength = 1 - strengthEffect;
        }

        var currentRound = buffRound.currentValue + round;
        if (baseStrength == 1) 
        {
            buffRound.SetValue(0);
        }
        else
        {
            buffRound.SetValue(currentRound);
        }
    }

    public void BuffAnimation()
    {
        buff.SetActive(true);
    }

    /// <summary>
    /// 回合转换时调用此事件函数 减少buff持续回合
    /// </summary>
    public void UpdateStrengthRound()
    {
        buffRound.SetValue(buffRound.currentValue - 1);
        if (buffRound.currentValue <= 0)
        {
            buffRound.SetValue(0);
            baseStrength = 1f;
        }
    }
}
