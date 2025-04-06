using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UnityEngine;

namespace Player
{
    class PlayerInventory
    {

        private Stack<IMineral> _inventory = new();

        public float MaxWeight { get; set; } = 30f;

        public int Count => _inventory.Count;

        public float CurrentWeight => _inventory.Sum(it => it.Size);

        public bool TryPush(IMineral item)
        {
            if (CurrentWeight + item.Size > MaxWeight)
            {
                Debug.Log($"Push failed: current weight: {CurrentWeight}, item width: {item.Size}, max weight: {MaxWeight}");
                return false;
            }

            _inventory.Push(item);
            Debug.Log($"Push success, current weight: {CurrentWeight}, max weight: {MaxWeight}");
            return true;
        }

        public IMineral Pop()
        {
            return _inventory.Pop();
        }

        public uint SellAllForMoney()
        {
            uint s = 0;
            while (_inventory.Count > 0)
                s += _inventory.Pop().Cost;

            return s;
        }

    }
}