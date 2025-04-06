using System;
using UnityEditor.Callbacks;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float currentHealth = 50f;

    public float maxHealth = 100f;

    public static event Action<float> currentHealthChanged;

    public int repairAmount = 2;

    public float speed = 5f;

    private bool isMoving = false;

    private Rigidbody2D rb;

    AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.linearVelocity = -transform.up * speed; // Более физически корректно
            audioSource.Play();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            audioSource.Stop();
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
                break;

            case Stage.Fight:
                isMoving = true;
                break;
        }
    }

    private void HandlePlatformRepaired()
    {
        currentHealth += repairAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        currentHealthChanged?.Invoke(currentHealth);
    }
}
