using Interfaces;

public interface IDamageable : IEntity, IState
{
    public float TakeDamage(float damage);
    
    public delegate void OnTakeDamageDelegate(float damage);
    public event OnTakeDamageDelegate onTakeDamageEvent;
    
    public delegate void OnDieDelegate();
    public event OnDieDelegate onDieEvent;
}
