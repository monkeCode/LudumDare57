using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public int currentHealth = 50;

    public int maxHealth = 100;

    public int repairAmount = 2;

    public float speed = 5f;

    private bool isMoving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.position -= new Vector3(0, speed, 0) * Time.deltaTime;
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
    }
}
