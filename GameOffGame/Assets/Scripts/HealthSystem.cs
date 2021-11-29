using System;

public class HealthSystem {

    public event EventHandler OnHealthChanged;

    private float health;
    private int healthMax;

    public HealthSystem(int health) {
        this.healthMax = health;
        this.health = healthMax;
    }

    public float GetHealth() {
        return health;
    }

    public float GetHealthPercent()
    {
        return health / healthMax;
    }

    public void Damage(int damageAmount) {
        health -= damageAmount;
        if (health <= 0)
            health = 0;

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Damage(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
            health = 0;

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(int healAmount) {
        health += healAmount;
        if (health > healthMax)
            health = healthMax;

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
        if (health > healthMax)
            health = healthMax;

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void IncreaseMaxHealth(int newMaxHealth)
    {
        health += newMaxHealth - healthMax;
        healthMax += newMaxHealth;
    }
}
