using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public Slider playerHealthBar;
    private HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = new HealthSystem(100);
        HealthBar healthBar = playerHealthBar.GetComponent<HealthBar>();
        healthBar.Setup(healthSystem);
        StartCoroutine(PassiveHealthRegenCoroutine(100, 500));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DoDamage(20);
        }
    }

    public float GetHealthPercent()
    {
        return healthSystem.GetHealthPercent();
    }

    public void DoDamage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    public void DamageOverTime(int damageAmount, int duration)
    {
        StartCoroutine(DamageOverTimeCoroutine(damageAmount, duration));
    }

    IEnumerator DamageOverTimeCoroutine(float damageAmount, float duration)
    {
        float amountDamaged = 0;
        float damagePerLoop = damageAmount / duration;
        while (amountDamaged < damageAmount)
        {
            healthSystem.Damage(damagePerLoop);
            amountDamaged += damagePerLoop;
            yield return new WaitForSeconds(1f);
        }
    }

    public void DoHeal(int healAmount)
    {
        healthSystem.Heal(healAmount);
    }

    public void HealOverTime(int healAmount, int duration)
    {
        StartCoroutine(HealOverTimeCoroutine(healAmount, duration));
    }

    IEnumerator HealOverTimeCoroutine(float healAmount, float duration)
    {
        float amountHealed = 0;
        float healPerLoop = healAmount / duration;
        while (amountHealed < healAmount)
        {
            healthSystem.Heal(healPerLoop);
            amountHealed += healPerLoop;
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator PassiveHealthRegenCoroutine(float healAmount, float duration)
    {
        float healPerLoop = healAmount / duration;
        while (true)
        {
            healthSystem.Heal(healPerLoop);
            yield return new WaitForSeconds(1f);
        }
    }
}
