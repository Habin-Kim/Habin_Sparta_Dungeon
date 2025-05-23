using System;
using UnityEngine;

public interface IDamagable
{ 
    void TakePhysicalDamage(int damageAmount);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;

    public Condition health { get { return uiCondition.health; } }
    public Condition stamina { get { return uiCondition.stamina; } }

    public float noStamina;

    public event Action OnTakeDamage;

    void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (stamina.curValue == 0f)
        {
            health.Subtract(noStamina * Time.deltaTime);
        }

        if (health.curValue == 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("사망");
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0f)
        {
            return false;
        }

        stamina.Subtract(amount);
        return true;
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        OnTakeDamage?.Invoke();
    }
}
