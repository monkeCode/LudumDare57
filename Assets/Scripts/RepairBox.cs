using System;
using System.Collections;
using UnityEngine;

public class RepairBox : MonoBehaviour
{
    public static event Action PlatformRepaired;

    private bool canRepair = true;

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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (canRepair && Input.GetKey(KeyCode.E))
            {
                PlatformRepaired?.Invoke();
                StartCoroutine(RepairSuspend(0.2f));
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
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
        canRepair = false;
        yield return new WaitForSeconds(timeSuspend);
        canRepair = true;
    }
}
