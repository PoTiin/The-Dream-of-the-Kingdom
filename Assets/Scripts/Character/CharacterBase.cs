using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int maxHp;
    public IntVariable hp;
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
    private bool isDead;
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    protected virtual void Start()
    {
        hp.maxValue = maxHp;
        CurrentHP = MaxHP;
    }

    public virtual void TakeDamage(int damage)
    {
        if(CurrentHP > damage)
        {
            CurrentHP -= damage;
            Debug.Log("CurrentHp:" + CurrentHP);
        }
        else
        {
            CurrentHP = 0;
            //当前人物死亡
            isDead = true;
        }
    }
}
