// 파일명: PlayerMovement.cs
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [field: SerializeField] public float MoveSpeed { get; private set; } = 6f;
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }
    
    
    [field: SerializeField] public string WalkParamName { get; private set; } = "IsWalking";

    private int hashWalk;
    private Animator playerAnimator;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Camera mainCamera;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        hashWalk = Animator.StringToHash(WalkParamName);
    }

    private void FixedUpdate()
    {
        Vector3 moveDir = new Vector3(playerInput.Horizontal, 0f, playerInput.Vertical).normalized;

        MoveCharacter(moveDir);
        RotateToMouse();
        UpdateAnimation(moveDir);
    }

    private void MoveCharacter(Vector3 moveDir)
    {
        Vector3 delta = moveDir * MoveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + delta);
    }

    private void RotateToMouse()
    {
        if (mainCamera == null) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, GroundLayer))
        {
            Vector3 lookDir = hit.point - transform.position;
            lookDir.y = 0f;
            if (lookDir != Vector3.zero)
            {
                playerRigidbody.MoveRotation(Quaternion.LookRotation(lookDir));
            }
        }
    }

    private void UpdateAnimation(Vector3 moveDir)
    {
        if (playerAnimator == null) return;
        playerAnimator.SetBool(hashWalk, IsMoving(moveDir));
    }

    private bool IsMoving(Vector3 moveDir)
    {
        return moveDir != Vector3.zero;
    }
}