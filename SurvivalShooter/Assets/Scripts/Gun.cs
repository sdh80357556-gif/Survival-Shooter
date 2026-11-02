
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [field: SerializeField] public Transform FireTransform { get; private set; }
    [field: SerializeField] public ParticleSystem MuzzleFlashEffect { get; private set; }
    [field: SerializeField] public GunData GunData { get; private set; }
    [field: SerializeField] public LayerMask TargetLayer { get; private set; }
    [field: SerializeField] public float FireDistance { get; private set; } = 50f;

    private LineRenderer bulletLineRenderer;
    private AudioSource gunAudioPlayer;
    private float lastFireTime;

    private void Awake()
    {
        bulletLineRenderer = GetComponent<LineRenderer>();
        gunAudioPlayer = GetComponent<AudioSource>();

        if (FireTransform == null) FireTransform = transform;
        if (MuzzleFlashEffect == null) MuzzleFlashEffect = GetComponentInChildren<ParticleSystem>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable() => lastFireTime = 0f;


    public void Fire()
    {
        if (Time.time >= lastFireTime + GunData.timeBetFire)
        {
            lastFireTime = Time.time;
            Shoot();
        }
    }

    private void Shoot()
    {
        Vector3 hitPosition;
        Ray ray = new Ray(FireTransform.position, FireTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, FireDistance, TargetLayer))
        {
            hitPosition = hit.point;
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                target.OnDamage(GunData.damage, hit.point, hit.normal);
            }
        }
        else
        {
            hitPosition = FireTransform.position + FireTransform.forward * FireDistance;
        }

        StartCoroutine(CoShotEffect(hitPosition));
    }

    private IEnumerator CoShotEffect(Vector3 hitPosition)
    {
        if (MuzzleFlashEffect != null) MuzzleFlashEffect.Play();
        if (GunData != null && GunData.shotClip != null) gunAudioPlayer.PlayOneShot(GunData.shotClip);

        bulletLineRenderer.SetPosition(0, FireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(0.03f);

        bulletLineRenderer.enabled = false;
    }
}