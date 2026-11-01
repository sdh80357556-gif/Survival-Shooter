// 파일명: LivingEntity.cs
using UnityEngine;
using UnityEngine.Events;

public class LivingEntity : MonoBehaviour, IDamageable
{
    [field: SerializeField] public float StartingHealth { get; protected set; } = 100f;
    public float Health { get; private set; }
    public bool IsDead { get; private set; }
    public UnityEvent OnDead;

    protected virtual void OnEnable()
    {
        IsDead = false;
        Health = StartingHealth;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead) return;

        Health -= damage;
        if (Health <= 0f)
        {
            Health = 0f;
            Die();
        }
    }

    public virtual void Die()
    {
        IsDead = true;
        if (OnDead != null) OnDead.Invoke();
    }
}