
namespace Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(uint damage);

        void Heal(uint heals);

        void Kill();
    }
}
