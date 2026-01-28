using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int maxHp;
    public IntVariable hp;

    public IntVariable defense;

    public IntVariable buffRound;
    public int CurrentHP
    {
        get => hp.currentValue;
        set
        {
            hp.SetValue(value);
        }
    }
    public int MaxHP
    {
        get => hp.maxValue;
    }

    protected Animator animator;
    public bool isDead;

    public GameObject buff;
    public GameObject debuff;

    //力量有关
    public float baseStrength = 1f;

    private float strengthEffect = 0.5f;
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    protected virtual void Start()
    {
        hp.maxValue = maxHp;
        CurrentHP = MaxHP;

        buffRound.currentValue = buffRound.maxValue;

        ResetDefense();
    }

    public virtual void TakeDamage(int damage)
    {
        var currentDamage = Mathf.Max(damage - defense.currentValue, 0);
        var currentDefense = Mathf.Max(defense.currentValue - damage, 0);
        defense.SetValue(currentDefense);
        if(CurrentHP > currentDamage)
        {
            CurrentHP -= currentDamage;
            //Debug.Log("CurrentHp:" + CurrentHP);
        }
        else
        {
            CurrentHP = 0;
            //当前人物死亡
            isDead = true;
        }
    }

    public void UpdateDefense(int amount)
    {
        var value = defense.currentValue + amount;
        defense.SetValue(value);
    }

    public void HealHealth(int amount)
    {
        CurrentHP = Mathf.Min(MaxHP,CurrentHP + amount);
        buff.SetActive(true);
    }
    public void ResetDefense()
    {
        defense.SetValue(0);
    }

    public void SetupStrength(int round, bool isPositive)
    {
        if (isPositive)
        {
            float newStrength = baseStrength + strengthEffect;
            baseStrength = Mathf.Min(newStrength, 1.5f);
            buff.SetActive(true);
        }
        else
        {
            debuff.SetActive(true);
            baseStrength = 1 - strengthEffect;
        }
        var currentRound = buffRound.currentValue + round;

        if (baseStrength == 1)
            buffRound.SetValue(0);
        else
            buffRound.SetValue(currentRound);
    }
    /// <summary>
    /// 回合转换事件函数
    /// </summary>
    public void UpdateStrengthRound()
    {
        buffRound.SetValue(buffRound.currentValue - 1);
        if(buffRound.currentValue <= 0)
        {
            buffRound.SetValue(0);
            baseStrength = 1;
        }
    }
}
