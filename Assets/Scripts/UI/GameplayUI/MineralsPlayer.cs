using GameResources;
using Player;
using TMPro;
using UnityEngine;

public class MineralsPlayer : MonoBehaviour
{
    public TMP_Text mineralsText;

    private CurrencyStorage _storage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _storage = FindFirstObjectByType<CurrencyStorage>();
    }

    // Update is called once per frame
    void Update()
    {
        mineralsText.text = $"{_storage.GetCount()}";
    }
}
