using Player;
using TMPro;
using UnityEngine;

public class HealthPlayer : MonoBehaviour
{
    public TMP_Text healthText;

    private Player.Player _player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = FindFirstObjectByType<Player.Player>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = $"{_player.Hp}/{_player.MaxHp}";
    }
}
