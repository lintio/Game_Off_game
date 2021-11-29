using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private HealthSystem healthSystem;
    [SerializeField] private Slider playerHealthBar;

    public void Setup(HealthSystem healthSystem) {
        this.healthSystem = healthSystem;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        if (playerHealthBar != null)
        {
            playerHealthBar.value = healthSystem.GetHealthPercent();
        }
        else
        {
            transform.Find("Bar").localScale = new Vector3(healthSystem.GetHealthPercent(), 15);
        }
    }
}
