// 파일명: IDamageable.cs
using UnityEngine;

public interface IDamageable
{
    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}