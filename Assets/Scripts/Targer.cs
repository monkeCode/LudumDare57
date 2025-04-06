using UnityEngine;
using UnityEngine.InputSystem;

public class Targer : MonoBehaviour
{
    [SerializeField] Sprite _target;
    [SerializeField] Sprite _reload;

    private SpriteRenderer _sp;

    void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var v =  Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        v.z = 0;
        transform.position = v;
        if(Player.Player.Instance.WeaponHandler.IsReloading)
            _sp.sprite = _reload;
        else
            _sp.sprite = _target;
    }
}
