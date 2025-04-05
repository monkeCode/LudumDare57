using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interfaces;

namespace Player
{
    class PlayerInventory
    {

        private Stack<IMineral> _inventory;
        public int Size;

        public int Count => _inventory.Count;

        public float Weight => _inventory.Sum(it => it.Size);

        public bool Push(IMineral item)
        {
            if (_inventory.Count == Size)
                return false;
            _inventory.Push(item);
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