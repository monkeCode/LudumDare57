using System.Collections.Generic;
using System.Linq;
using Interfaces;

namespace Player
{
    class PlayerInventory
    {

        private Stack<IMineral> _inventory;

        public float MaxWeight {get;set;}

        public int Count => _inventory.Count;

        public float CurrentWegiht => _inventory.Sum(it => it.Size);

        public bool Push(IMineral item)
        {
            if (CurrentWegiht + item.Size > MaxWeight)
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