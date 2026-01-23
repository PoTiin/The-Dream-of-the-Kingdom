using UnityEngine;

public class Player : CharacterBase
{
    public IntVariable playerMana;
    public int maxMana;
    public int CurrentMana
    {
        get => playerMana.currentValue;
        set => playerMana.SetValue(value);
    }

    private void OnEnable()
    {
        playerMana.maxValue = maxMana;
        CurrentMana = playerMana.maxValue;//设置初始法力值
    }
    /// <summary>
    /// 监听事件函数
    /// </summary>
    public void NewTurn()
    {
        CurrentMana = maxMana;
    }
    public void UpdateMana(int cost)
    {
        CurrentMana -= cost++;
        if (CurrentMana <= 0)
        {
            CurrentMana = 0;
        }
    }
}
