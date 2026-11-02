
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