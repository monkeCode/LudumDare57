using GameResources;
using TMPro;
using UnityEngine;

public enum InteractState
{
    None,
    Repair,
}


public class InteractPlayer : MonoBehaviour
{
    public TMP_Text interactText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        interactText.text = $"[E] Repair";
    }

    // private void OnEnable()
    // {
    //     RepairBox.StateChanged += HandleStateChanged;
    //     HandleStateChanged(Timer.CurrentStage);
    // }

    // private void OnDisable()
    // {
    //     RepairBox.StateChanged -= HandleStateChanged;
    // }
}
