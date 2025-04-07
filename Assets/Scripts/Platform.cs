using System;
using System.Collections;
using GameResources;
using Interfaces;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        print($"timer instaance{Timer.instance}");
        Timer.instance.StageChanged += HandleStageChanged;
        RepairBox.PlatformRepaired += HandlePlatformRepaired;
        HandleStageChanged(Timer.instance.CurrentStage);
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameManager.Instance.CurrentStage == Stage.Fight)
            MoveToPosition();
    }

    private void HandleStageChanged(Stage newStage)
    {
        switch (newStage)
        {
            case Stage.Clill:
                isMoving = false;
                audioSource?.Stop();
                break;

            case Stage.Fight:
                isMoving = true;
                speed = (GameManager.Instance.StagePoints[GameManager.Instance.CountStage].y - transform.position.y)/Timer.instance.timeForFighting;
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
        Die();
    }

    public void Die()
    {

    }

    
    void MoveToPosition()
    {
        var npos = transform.position;
        npos.y += speed * Time.deltaTime;
        rb.MovePosition(npos);
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2f);
        //SceneManager.LoadScene();
    }
}
