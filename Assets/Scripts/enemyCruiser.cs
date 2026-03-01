using UnityEngine;

public sealed class AIPlane : MonoBehaviour
{
    [SerializeField] private float cruiseSpeedKmh = 100f;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.linearDamping = 0f;
        _rb.angularDamping = 0f;
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = transform.forward * (cruiseSpeedKmh / 3.6f);
    }
}