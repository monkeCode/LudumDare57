using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public Platform platform;

    void Start()
    {
        healthBar = GetComponent<Image>();
        platform = FindFirstObjectByType<Platform>();
    }


    void Update()
    {
        healthBar.fillAmount = platform.currentHealth / platform.maxHealth;
    }
}
