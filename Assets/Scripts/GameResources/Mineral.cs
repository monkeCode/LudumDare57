
using System;
using Interfaces;
using UnityEngine;

namespace GameResources
{
    public class Mineral: MonoBehaviour, IMineral
    {
        [SerializeField] private float size = 1;
        [SerializeField] private uint cost = 1;
        [SerializeField] private bool isInteractable;
        [SerializeField] private Player.Player Player;
        public float Size  => size;
        public uint Cost => cost;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && isInteractable && Player.inventory.TryPush(this))
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out Player.Player player))
            {
                isInteractable = true;
                Player = player;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out Player.Player player))
            {
                isInteractable = false;
            }
        }
    }
}