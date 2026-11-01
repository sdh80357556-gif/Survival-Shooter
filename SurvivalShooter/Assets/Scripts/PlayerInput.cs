using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string VerticalAxis = "Vertical";
    public static readonly string HorizontalAxis = "Horizontal";
    public static readonly string FireButton = "Fire1";

    public float Vertical { get; private set; }
    public float Horizontal { get; private set; }
    public bool Fire { get; private set; }

    private void OnDisable()
    {
        Vertical = 0f;
        Horizontal = 0f;
        Fire = false;
    }

    private void Update()
    {
        Vertical = Input.GetAxisRaw(VerticalAxis);
        Horizontal = Input.GetAxisRaw(HorizontalAxis);
        Fire = Input.GetButton(FireButton);
    }
}