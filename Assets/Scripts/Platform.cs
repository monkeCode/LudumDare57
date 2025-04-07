using System;
using System.Collections;
using Core;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Platform : MonoBehaviour, IDamageable
{
    public float currentHealth = 50f;

    public float maxHealth = 100f;

    public static event Action<float> currentHealthChanged;

    public float speed = 5f;

    private bool isMoving = false;

    private Rigidbody2D rb;
    public static Platform Instance { get; private set; }

    public GameObject PlatformRespawnPoint;

    AudioSource audioSource;

    public GameObject fadePanel;
    public float fadeDuration = 2.0f;

    [SerializeField] private Highlighter _repairHighlighter;

    bool dying = false;

    bool start = true;


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
        //HandleStageChanged(Timer.instance.CurrentStage);
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        fadePanel.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving)
            MoveToPosition();
        if (currentHealth <= 0 && !dying)
        {
            Die();
        }
    }

    private void HandleStageChanged(Stage newStage)
    {
        switch (newStage)
        {
            case Stage.Clill:
                isMoving = false;
                audioSource?.Stop();
                isMoving = false;
                break;

            case Stage.Fight:
                isMoving = true;
                speed = (GameManager.Instance.StagePoints[GameManager.Instance.CountStage].y - transform.position.y) / Timer.instance.timeForFighting;
                audioSource?.Play();
                isMoving = true;
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
        _repairHighlighter.Highlight();
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
        dying = true;
        StopAllCoroutines();
        Timer.instance.GetComponent<AudioSource>().Stop();
        fadePanel.SetActive(true);
        StartCoroutine(FadeIn());
    }


    void MoveToPosition()
    {
        var npos = transform.position;
        npos.y += speed * Time.deltaTime;
        rb.MovePosition(npos);
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0;
        Image fadeImage = fadePanel.GetComponent<Image>();
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            color.a = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            fadeImage.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }
        print(color);
        color.a = 1;
        fadeImage.color = color;
        SceneManager.LoadScene("GameOver");
    }
}
