namespace Interfaces.MilitaryGame
{
    public interface IDamageable
    {
        public bool IsAlive();
        public void TakeDamage(int damage);
    }
}
