using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "Scriptable Objects/ZombieData")]
public class ZombieData : ScriptableObject
{
    public float maxHp = 100f;
    public float damage = 15f;
    public float speed = 3f;
}