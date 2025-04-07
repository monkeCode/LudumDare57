using System;
using System.Collections;
using GameResources;
using Interfaces;
using UnityEditor.Callbacks;
using UnityEngine;

public class Platform : MonoBehaviour, IDamageable
{
    public float currentHealth = 50f;

    public float maxHealth = 100f;

    public static event Action<float> currentHealthChanged;

    public float speed = 5f;

    private bool isMoving = false;

    private Rigidbody2D rb;
    public static Platform Instance { get; private set; }

    AudioSource audioSource;

    public int currentFloor = 0;


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
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void OnEnable()
    {
        Timer.instance.StageChanged += HandleStageChanged;
        RepairBox.PlatformRepaired += HandlePlatformRepaired;
        HandleStageChanged(Timer.instance.CurrentStage);
    }

    private void OnDisable()
    {
        Timer.instance.StageChanged -= HandleStageChanged;
        RepairBox.PlatformRepaired -= HandlePlatformRepaired;
    }

    private void HandleStageChanged(Stage newStage)
    {
        switch (newStage)
        {
            case Stage.Clill:
                isMoving = false;
                audioSource?.Stop();
                currentFloor += 1;
                break;

            case Stage.Fight:
                isMoving = true;
                StartCoroutine(MoveToPosition(GameManager.Instance.StagePoints[currentFloor], Timer.instance.timeForFighting));
                audioSource.Play();
                break;
        }
    }

    private void HandlePlatformRepaired(int amount)
    {
        if (currentHealth >= maxHealth) return;

        currentHealth += amount;

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

    IEnumerator MoveToPosition(Vector3 targetPos, float time)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < time)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }
}
