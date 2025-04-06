using System;
using GameResources;
using Interfaces;
using UnityEditor.Callbacks;
using UnityEngine;

public class Platform : MonoBehaviour, IDamageable
{
    public float currentHealth = 50f;

    public float maxHealth = 100f;

    public static event Action<float> currentHealthChanged;

    public int repairAmount = 2;

    public float speed = 5f;

    private bool isMoving = false;

    private Rigidbody2D rb;
    public static Platform Instance { get; private set; }

    private CurrencyStorage _currencyStorage;

    AudioSource audioSource;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        _currencyStorage = FindFirstObjectByType<CurrencyStorage>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.linearVelocity = -transform.up * speed; // Более физически корректно
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnEnable()
    {
        Timer.StageChanged += HandleStageChanged;
        RepairBox.PlatformRepaired += HandlePlatformRepaired;
        HandleStageChanged(Timer.CurrentStage);
    }

    private void OnDisable()
    {
        Timer.StageChanged -= HandleStageChanged;
        RepairBox.PlatformRepaired -= HandlePlatformRepaired;
    }

    private void HandleStageChanged(Stage newStage)
    {
        switch (newStage)
        {
            case Stage.Clill:
                isMoving = false;
                audioSource.Stop();
                break;

            case Stage.Fight:
                isMoving = true;
                audioSource.Play();
                break;
        }
    }

    private void HandlePlatformRepaired()
    {
        if(currentHealth >= maxHealth || !_currencyStorage.TrySpendCurrency(repairAmount)) return;

        currentHealth += repairAmount;
        
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        currentHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(uint damage)
    {
        currentHealth -= damage;
        currentHealth = Math.Clamp(currentHealth, 0, maxHealth);
        currentHealthChanged?.Invoke(currentHealth);
    }

    public void Heal(uint heals)
    {
        currentHealth += heals;
        currentHealth = Math.Clamp(currentHealth, 0, maxHealth);
        currentHealthChanged?.Invoke(currentHealth);
    }
    public void Kill()
    {
        
    }
}
