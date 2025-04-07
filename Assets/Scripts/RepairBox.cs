using System;
using System.Collections;
using Core;
using UnityEngine;
using GameResources;

public class RepairBox : MonoBehaviour, IInteractable
{
    public static event Action<int> PlatformRepaired;

    private bool _canRepair = true;

    AudioSource audioSource;

    public int repairAmount = 2;

    private CurrencyStorage _currencyStorage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _currencyStorage = FindFirstObjectByType<CurrencyStorage>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Interact()
    {
        if (_canRepair && Platform.Instance.currentHealth < Platform.Instance.maxHealth && _currencyStorage.TrySpendCurrency(repairAmount))
        {

            PlatformRepaired?.Invoke(repairAmount);
            StartCoroutine(RepairSuspend(0.2f));
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    private IEnumerator RepairSuspend(float timeSuspend)
    {
        _canRepair = false;
        yield return new WaitForSeconds(timeSuspend);
        _canRepair = true;
    }
}
