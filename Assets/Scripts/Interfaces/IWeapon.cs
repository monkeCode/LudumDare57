
using UnityEngine;

namespace Interfaces
{
interface IWeapon
{
    void ShootPress(Vector2 point, Vector2 direction);

    void ShootRelease(Vector2 point, Vector2 direction);

    void Reload();

}

}
