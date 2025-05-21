using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    public Condition health { get { return uiCondition.health; } }
    public Condition stamina { get { return uiCondition.stamina; } }

    public float noStamina;

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
        Debug.Log("Player 다운!");
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
}
