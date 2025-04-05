
namespace Interfaces
{
    public interface IDamageable
    {

        uint MaxHp {get;}

        uint Hp {get; protected set;}

        void TakeDamage(uint damage);

        void Kill();
    }
}
