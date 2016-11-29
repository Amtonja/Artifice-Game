namespace Artiface.Interfaces {
    public interface IHealth<T> {
        T Health { get; }

        void TakeDamage(T _damage);
        void Heal(T _health);
        void Die();
    } 
}
