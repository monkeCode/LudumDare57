
using System;
using Core;
using Interfaces;
using UnityEngine;

namespace GameResources
{
    [RequireComponent(typeof(AudioSource))]
    public class Mineral: MonoBehaviour, IMineral, IInteractable
    {
        [SerializeField] private float size = 1;
        [SerializeField] private uint cost = 1;
        [SerializeField] private bool isInteractable;
        [SerializeField] private Player.Player Player;

        private bool _collected = false;
        
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
            if(_collected) 
                return;
            if (Player.inventory.TryPush(this))
            {
                _collected = true;
                GetComponent<AudioSource>().Play();
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                Destroy(gameObject, 5);
            }
        }
    }
}