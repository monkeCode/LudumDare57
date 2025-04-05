using System;
using System.Collections;
using UnityEngine;

public class RepairBox : MonoBehaviour
{
    public static event Action PlatformRepaired;

    private bool canRepair = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //    if (other.CompareTag("Player")) 
        //    {
        if (canRepair && Input.GetKey(KeyCode.E))
        {
            PlatformRepaired?.Invoke();
            StartCoroutine(RepairSuspend(0.2f));
        }
        //    }
    }

    private IEnumerator RepairSuspend(float timeSuspend)
    {
        canRepair = false;
        yield return new WaitForSeconds(timeSuspend);
        canRepair = true;
    }
}
