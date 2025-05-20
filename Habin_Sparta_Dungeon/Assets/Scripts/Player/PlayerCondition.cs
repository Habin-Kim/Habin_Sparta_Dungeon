using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

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
}
