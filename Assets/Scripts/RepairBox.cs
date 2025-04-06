using System;
using System.Collections;
using Core;
using UnityEngine;

public class RepairBox : MonoBehaviour, IInteractable
{
    public static event Action PlatformRepaired;

    private bool _canRepair = true;

    public void Interact()
    {
        if (_canRepair)
        {
            PlatformRepaired?.Invoke();
            StartCoroutine(RepairSuspend(0.2f));
        }
    }

    private IEnumerator RepairSuspend(float timeSuspend)
    {
        _canRepair = false;
        yield return new WaitForSeconds(timeSuspend);
        _canRepair = true;
    }
}
