using System;
using System.Collections;
using Core;
using UnityEngine;

public class RepairBox : MonoBehaviour, IInteractable
{
    public static event Action PlatformRepaired;

    private bool _canRepair = true;

    AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKey(KeyCode.E))
        {
            audioSource.Stop();
        }
    }


    public void Interact()
    {
        if (_canRepair)
        {

            PlatformRepaired?.Invoke();
            StartCoroutine(RepairSuspend(0.2f));
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioSource.Stop();
        }
    }

    private IEnumerator RepairSuspend(float timeSuspend)
    {
        _canRepair = false;
        yield return new WaitForSeconds(timeSuspend);
        _canRepair = true;
    }
}
