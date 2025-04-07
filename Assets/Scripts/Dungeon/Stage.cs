using System;
using Weapons;

namespace Dungeon
{
    [Serializable]
    public struct GeneratorStage
    {
        public Rarity WeaponRarity;
        public int WeaponsCount;
        public int MineralsCount;
    }
}