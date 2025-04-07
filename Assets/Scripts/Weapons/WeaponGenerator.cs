using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    class WeaponGenerator:MonoBehaviour
    {
        [SerializeField] private List<BaseWeapon> _weapons;

        public static WeaponGenerator Instance {get;private set;}

        void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public BaseWeapon GenerateWeapon(Rarity rarity)
        {
            return _weapons[Random.Range(0, _weapons.Count)].GenerateCopy(rarity);
        }

        public BaseWeapon GenerateCommon()
        {
            return GenerateWeapon(Rarity.Common);
        }

        public BaseWeapon GenerateUncommon()
        {
            return GenerateWeapon(Rarity.Uncommon);
        }
        public BaseWeapon GenerateRare()
        {
            return GenerateWeapon(Rarity.Rare);
        }
    }

}