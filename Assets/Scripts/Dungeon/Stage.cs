using System;
using System.Collections.Generic;
using UnityEngine;
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

    [Serializable]
    public struct FightStage
    {
        [Serializable]
        public struct FightOption
        {
            public GameObject entity;
            public int count;
        }
        public FightOption[] entities;
    }

    [Serializable]
    public struct DungeonEntities
    {
        [Serializable]
        public struct Entity
        {
            public GameObject entity;
            public int minCountPoint;
            public int maxCountPoint;
            public readonly int Count => UnityEngine.Random.Range(minCountPoint, maxCountPoint);
        }

        public List<Entity> entities;

    }
}