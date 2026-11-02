using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [field: SerializeField] public Transform Target { get; private set; }
    [field: SerializeField] public float SmoothSpeed { get; private set; } = 5f;

    private Vector3 offset;

    private void Start()
    {
        if (Target != null) offset = GetOffset(transform.position, Target.position);
    }

    private void FixedUpdate()
    {
        if (Target == null) return;
        transform.position = Vector3.Lerp(transform.position, Target.position + offset, SmoothSpeed * Time.deltaTime);
    }


    private Vector3 GetOffset(Vector3 camPos, Vector3 targetPos)
    {
        return camPos - targetPos;
    }
}