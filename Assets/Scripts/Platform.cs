using System;
using UnityEditor.Callbacks;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public int currentHealth = 50;

    public int maxHealth = 100;

    public static event Action<int> currentHealthChanged;

    public int repairAmount = 2;

    public float speed = 5f;

    private bool isMoving = false;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving)
        {
            //transform.position -= new Vector3(0, speed, 0) * Time.deltaTime;
            Vector2 movement = -transform.up * speed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
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
