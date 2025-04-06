
using System;
using Core;
using Interfaces;
using UnityEngine;

namespace GameResources
{
    public class Mineral: MonoBehaviour, IMineral, IInteractable
    {
        [SerializeField] private float size = 1;
        [SerializeField] private uint cost = 1;
        [SerializeField] private bool isInteractable;
        [SerializeField] private Player.Player Player;
        public float Size
        {
            get => size;
            set => size = value;
        }

        private void Start()
        {
            Player = FindFirstObjectByType<Player.Player>();
        }

        public uint Cost
        {
            get => cost;
            set => cost = value;
        }

        public void Interact()
        {
            if (Player.inventory.TryPush(this))
            {
                Destroy(gameObject);
            }
        }
    }
}