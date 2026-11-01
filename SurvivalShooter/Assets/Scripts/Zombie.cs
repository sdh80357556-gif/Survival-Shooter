// 파일명: Zombie.cs
using System.Text;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Die,
    }

    [field: SerializeField] public Transform Target { get; private set; }
    [field: SerializeField] public LayerMask TargetLayer { get; private set; }
    [field: SerializeField] public float TraceDistance { get; private set; } = 20f;
    [field: SerializeField] public float AttackDistance { get; private set; } = 1.5f;
    [field: SerializeField] public float Damage { get; private set; } = 10f;
    [field: SerializeField] public float AttackInterval { get; private set; } = 1f;
    [field: SerializeField] public string DieParamName { get; private set; } = "Dead";

    [field: SerializeField] public ParticleSystem HitEffect { get; private set; }
    [field: SerializeField] public Collider ZombieCollider { get; private set; }
    [field: SerializeField] public AudioClip HitClip { get; private set; }
    [field: SerializeField] public AudioClip DeathClip { get; private set; }

    private NavMeshAgent agent;
    private Animator zombieAnimator;
    private AudioSource zombieAudioSource;

    private int idDie;
    private float lastAttackTime;
    private Status currentStatus;
    public static readonly int IdHasTarget = Animator.StringToHash("HasTarget");
    public static readonly int IdDie = Animator.StringToHash("Die");

    public Status CurrentStatus
    {
        get => currentStatus;
        set
        {
            currentStatus = value;
            switch (currentStatus)
            {
                case Status.Idle:
                    if (agent != null && agent.isOnNavMesh) agent.isStopped = true;
                    break;
                case Status.Trace:
                    if (agent != null && agent.isOnNavMesh) agent.isStopped = false;
                    zombieAnimator.SetBool(IdHasTarget, true);
                    break;
                case Status.Attack:
                    if (agent != null && agent.isOnNavMesh) agent.isStopped = true;
                    break;
                case Status.Die:
                    if (agent != null && agent.isOnNavMesh) agent.isStopped = true;
                    if (zombieAnimator != null) zombieAnimator.SetTrigger(IdDie);
                    if (ZombieCollider != null) ZombieCollider.enabled = false;
                    if (DeathClip != null) zombieAudioSource.PlayOneShot(DeathClip);
                    zombieAnimator.SetTrigger(IdDie);
                    break;
            }
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioSource = GetComponent<AudioSource>();

        if (ZombieCollider == null) ZombieCollider = GetComponent<Collider>();
        if (HitEffect == null) HitEffect = GetComponentInChildren<ParticleSystem>();

        idDie = Animator.StringToHash(DieParamName);
    }

    public void Setup(ZombieData data)
    {
        StartingHealth = data.maxHp;
        Damage = data.damage;
        if (agent != null) agent.speed = data.speed;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
        if (ZombieCollider != null) ZombieCollider.enabled = true;
        CurrentStatus = Status.Idle;
    }

    private void OnDisable()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.ResetPath();
        }
    }

    private void Update()
    {
        switch (CurrentStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;
            case Status.Trace:
                UpdateTrace();
                break;
            case Status.Attack:
                UpdateAttack();
                break;
            case Status.Die:
                UpdateDie();
                break;
        }
    }

    private void UpdateDie() => transform.Translate(Vector3.down * 0.5f * Time.deltaTime);

    private void UpdateIdle()
    {
        if (IsTargetInRange(TraceDistance))
        {
            CurrentStatus = Status.Trace;
            return;
        }
        Target = FindTarget(TraceDistance);
    }

    private void UpdateTrace()
    {
        if (IsTargetInRange(AttackDistance))
        {
            CurrentStatus = Status.Attack;
            return;
        }
        if (!IsTargetInRange(TraceDistance))
        {
            Target = null;
            CurrentStatus = Status.Idle;
            return;
        }
        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(Target.position);
        }
    }

    private void UpdateAttack()
    {
        if (!IsTargetInRange(AttackDistance))
        {
            CurrentStatus = Status.Trace;
            return;
        }
        if (Time.time >= lastAttackTime + AttackInterval)
        {
            lastAttackTime = Time.time;
            IDamageable damageable = Target.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Vector3 hitNormal = (transform.position - Target.position).normalized;
                damageable.OnDamage(Damage, Target.position, hitNormal);
            }
        }
    }

    
    private bool IsTargetInRange(float range)
    {
        return Target != null && Vector3.Distance(transform.position, Target.position) <= range;
    }

    private Transform FindTarget(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, TargetLayer);
        Transform closest = null;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < colliders.Length; ++i)
        {
            float distance = Vector3.Distance(transform.position, colliders[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = colliders[i].transform;
            }
        }
        return closest;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead) return;
        base.OnDamage(damage, hitPoint, hitNormal);

        if (HitClip != null) zombieAudioSource.PlayOneShot(HitClip);
        if (HitEffect != null)
        {
            HitEffect.transform.position = hitPoint;
            HitEffect.transform.forward = hitNormal;
            HitEffect.Play();
        }
    }

    public override void Die()
    {
        base.Die();
        CurrentStatus = Status.Die;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"[몬스터: {gameObject.name}]");
        sb.Append("상태: ").AppendLine(CurrentStatus.ToString());
        sb.Append("체력: ").Append(Health);
        return sb.ToString();
    }
}