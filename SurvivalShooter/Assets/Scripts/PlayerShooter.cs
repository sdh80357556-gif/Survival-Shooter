// 파일명: PlayerShooter.cs (왼손/오른손 IK와 재장전을 제거한 초간단 버전!)
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [field: SerializeField] public Gun Gun { get; private set; }

    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (Gun == null) Gun = GetComponentInChildren<Gun>();
    }

    private void Update()
    {
        if (playerInput != null && playerInput.Fire && Gun != null)
        {
            Gun.Fire();
        }
    }
}