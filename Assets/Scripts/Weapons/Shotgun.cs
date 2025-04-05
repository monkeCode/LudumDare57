using UnityEngine;
using Weapons;


namespace Weapons
{
    [CreateAssetMenu(menuName = "Weapons/Shotgun")]
    public class Shotgun : BaseWeapon
    {
        [SerializeField] private uint bullets_count;

        public override void ShootPress(Vector2 point, Vector2 direction)
        {
            if (!CanShoot())
            {
                return;
            }
            for(int i =0; i < bullets_count; i++)
                Shoot(point, direction);

            if (ShootMode == ShootMode.Single)
                Shooted = true;

        }
    }
}