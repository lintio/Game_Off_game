using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    const string LEFT = "left";
    const string RIGHT = "right";

    [Header("Componants")]
    AiAgent agent;

    [SerializeField] private int MaxHealth;
    public Transform pf_HealthBar;
    private HealthSystem healthSystem;
    private Transform healthBarTransform;
    Vector3 HealthBarScale;
    Vector3 healthBarNewScale;

    [Header("Movement Variables")]
    [SerializeField] private Transform castPos;
    [SerializeField] private Transform eyePos;

    [SerializeField] private int damageAmount = 10;

    private void Awake()
    {
        
    }

    private void Start()
    {
        agent = GetComponent<AiAgent>();
        agent.config.castPos = this.castPos;
        agent.config.eyePos = this.eyePos;
        // Healthbar stuff
        healthSystem = new HealthSystem(MaxHealth);
        healthBarTransform = Instantiate(pf_HealthBar, this.transform);
        Vector3 healthBarLocalPosition = new Vector3(0, 1);
        healthBarTransform.localPosition = healthBarLocalPosition;
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.Setup(healthSystem);
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        HealthBarScale = healthBarTransform.localScale;
        healthBarNewScale = HealthBarScale;
        DoDamage(50);
    }

    private void Update()
    {
        
        if (agent.config.facingDirection == LEFT)
        {
            healthBarNewScale.x = -HealthBarScale.x;
        }
        else
        {
            healthBarNewScale.x = HealthBarScale.x;
        }
        healthBarTransform.localScale = healthBarNewScale;

        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerManager>().DoDamage(damageAmount);
            agent.rb2d.velocity = new Vector2(0, agent.rb2d.velocity.y);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    #region DamageFunctions

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        if(healthSystem.GetHealth() <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float GetHealth()
    {
        return healthSystem.GetHealth();
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
    #endregion //DamageFunctions
}
