using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int maxHp;
    public IntVariable hp;

    public IntVariable defense;
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
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    protected virtual void Start()
    {
        hp.maxValue = maxHp;
        CurrentHP = MaxHP;

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

    public void ResetDefense()
    {
        defense.SetValue(0);
    }
}
