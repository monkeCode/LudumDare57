using System.Collections.Generic;
using Dungeon;
using UnityEngine;

public class EntityPoint : MonoBehaviour
{
    private bool _generated;

    private DungeonEntities _entities;
    [SerializeField] private float _spawnRange;
    [SerializeField] private LayerMask _groundMask;

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(_generated);
        if (_generated)
            return;

        if (collision.gameObject == Player.Player.Instance.gameObject)
        {
            foreach (var e in _entities.entities)
            {
                int count = e.Count;
                for (int i = 0; i < count; i++)
                {
                    Vector2 spawnPosition = FindValidSpawnPosition();
                    Instantiate(e.entity, spawnPosition, Quaternion.identity);
                }
            }
        }
        _generated = true;
    }

    Vector2 FindValidSpawnPosition(int maxAttempts = 10)
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomPosition = (Vector2)transform.position + Random.insideUnitCircle * _spawnRange;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(randomPosition, 5f, _groundMask);
            if (colliders.Length == 0)
            {
                return randomPosition;
            }
        }

        Debug.LogWarning("Не удалось найти свободное место для спавна!");
        return (Vector2)transform.position;
    }

    public void SetEntities(DungeonEntities entities)
    {
        _entities = entities;
    }


}
