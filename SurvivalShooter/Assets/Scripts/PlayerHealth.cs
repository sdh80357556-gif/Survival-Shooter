// 파일명: PlayerHealth.cs
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public static readonly int IdDie = Animator.StringToHash("Die");

    [field: SerializeField] public Image UiHealth { get; private set; }
    [field: SerializeField] public AudioClip HitClip { get; private set; }
    [field: SerializeField] public AudioClip DeathClip { get; private set; }

    private AudioSource playerAudioSource;
    private Animator playerAnimator;
    private PlayerMovement playerMovement;
    private PlayerInput playerInput;
    private PlayerShooter playerShooter;

    private void Awake()
    {
        playerAudioSource = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();
        playerShooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (playerMovement != null) playerMovement.enabled = true;
        if (playerInput != null) playerInput.enabled = true;
        if (playerShooter != null) playerShooter.enabled = true;

        if (UiHealth != null)
        {
            UiHealth.gameObject.SetActive(true);
            UiHealth.fillAmount = Health / StartingHealth;
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!IsDead && HitClip != null)
        {
            playerAudioSource.PlayOneShot(HitClip);
        }

        base.OnDamage(damage, hitPoint, hitNormal);
        if (UiHealth != null)
        {
            UiHealth.fillAmount = Health / StartingHealth;
        }
    }

    public override void Die()
    {
        base.Die();

        if (playerAnimator != null) playerAnimator.SetTrigger(IdDie);
        if (DeathClip != null) playerAudioSource.PlayOneShot(DeathClip);

        if (playerMovement != null) playerMovement.enabled = false;
        if (playerInput != null) playerInput.enabled = false;
        if (playerShooter != null) playerShooter.enabled = false;
    }
}