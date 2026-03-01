using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class PlaneController : MonoBehaviour
{
    [Header("Engine")]
    [SerializeField] private float accelMsPerSec = 25f;
    [SerializeField] private float decelMsPerSec = 20f;
    [SerializeField] private float maxSpeedKmh = 600f;

    [Header("Lift")]
    [SerializeField] private float takeoffSpeedKmh = 140f;
    [SerializeField] private float fullLiftSpeedKmh = 200f;
    [SerializeField] private float liftMultiplier = 1.1f;

    [Header("Controls")]
    [SerializeField] private float pitchSpeed = 90f;    // graus/seg
    [SerializeField] private float rollSpeed  = 130f;   // graus/seg
    [SerializeField] private float yawSpeed   = 45f;    // graus/seg
    [SerializeField] private float controlFullAtKmh = 120f;

    [Header("Ground")]
    [SerializeField] private float groundedRayDistance = 1.5f;

    [Header("Debug")]
    [SerializeField] private bool logSpeed = true;
    [SerializeField] private float logEverySeconds = 0.5f;

    private Rigidbody _rb;
    private float _targetSpeedMs;
    private bool _isGrounded;
    private float _logTimer;

    // adiciona no PlaneController
    public float TargetSpeedMs => _targetSpeedMs;
    public float MaxSpeedMs => KmhToMs(maxSpeedKmh);

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = true;
        _rb.linearDamping = 0f;
        _rb.angularDamping = 0f;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, groundedRayDistance);

        HandleSpeed();
        ApplyForwardVelocity();
        ApplyLift();
        ApplyRotation();

        if (_isGrounded)
            ApplyGroundStick();

        LogSpeed();
    }

    // ── Throttle ─────────────────────────────────────────────
    private void HandleSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            _targetSpeedMs += accelMsPerSec * Time.fixedDeltaTime;

        if (Input.GetKey(KeyCode.LeftControl))
            _targetSpeedMs -= decelMsPerSec * Time.fixedDeltaTime;

        _targetSpeedMs = Mathf.Clamp(_targetSpeedMs, 0f, KmhToMs(maxSpeedKmh));
    }

    // ── Velocidade sempre na direção do nariz ─────────────────
    // Isso é o segredo: o avião sempre voa pra onde o nariz aponta.
    // Quando você faz pitch up o nariz sobe, e a velocidade vai junto.
    private void ApplyForwardVelocity()
    {
        float currentSpeed = Vector3.Dot(_rb.linearVelocity, transform.forward);
        float newSpeed = Mathf.MoveTowards(currentSpeed, _targetSpeedMs, accelMsPerSec * Time.fixedDeltaTime);

        // Substitui a velocidade inteira pela direção atual do nariz
        // Isso faz o avião "virar" junto com a rotação, como deve ser
        _rb.linearVelocity = transform.forward * newSpeed;
    }

    // ── Lift ─────────────────────────────────────────────────
    private void ApplyLift()
    {
        if (_isGrounded) return;

        float speed = _rb.linearVelocity.magnitude;
        float lift01 = Mathf.InverseLerp(KmhToMs(takeoffSpeedKmh), KmhToMs(fullLiftSpeedKmh), speed);
        float weight = _rb.mass * Physics.gravity.magnitude;
        _rb.AddForce(Vector3.up * weight * lift01 * liftMultiplier, ForceMode.Force);
    }

    // ── Rotação nos 3 eixos ───────────────────────────────────
    private void ApplyRotation()
    {
        // Pitch: S = nariz sobe | W = nariz desce
        float pitch = (Input.GetKey(KeyCode.W) ?  1f : 0f)
                    + (Input.GetKey(KeyCode.S) ? -1f : 0f);

        // Roll: A = inclina esquerda | D = inclina direita
        float roll  = (Input.GetKey(KeyCode.A) ?  1f : 0f)
                    + (Input.GetKey(KeyCode.D) ? -1f : 0f);

        // Yaw: Q = nariz pra esquerda | E = nariz pra direita
        float yaw   = (Input.GetKey(KeyCode.Q) ? -1f : 0f)
                    + (Input.GetKey(KeyCode.E) ?  1f : 0f);

        // Autoridade dos controles aumenta com a velocidade
        float speed = _rb.linearVelocity.magnitude;
        float authority = Mathf.Clamp01(speed / KmhToMs(controlFullAtKmh));

        float dt = Time.fixedDeltaTime;
        transform.Rotate(
            pitch * pitchSpeed * authority * dt,   // eixo X local
            yaw   * yawSpeed   * authority * dt,   // eixo Y local
            roll  * rollSpeed  * authority * dt,   // eixo Z local
            Space.Self
        );

        _rb.angularVelocity = Vector3.zero;
    }

    // ── Comportamento no solo ─────────────────────────────────
    private void ApplyGroundStick()
    {
        // Nao deixa afundar no chao
        Vector3 v = _rb.linearVelocity;
        if (v.y < 0f) v.y = 0f;
        _rb.linearVelocity = v;

        // Nivela o aviao automaticamente enquanto esta no chao
        Vector3 euler = transform.eulerAngles;
        euler.x = Mathf.LerpAngle(euler.x, 0f, 0.15f);
        euler.z = Mathf.LerpAngle(euler.z, 0f, 0.15f);
        transform.eulerAngles = euler;
    }

    private void LogSpeed()
    {
        if (!logSpeed) return;
        _logTimer += Time.fixedDeltaTime;
        if (_logTimer < logEverySeconds) return;
        _logTimer = 0f;
        Debug.Log($"Speed: {_rb.linearVelocity.magnitude * 3.6f:0} km/h | Target: {_targetSpeedMs * 3.6f:0} km/h | Grounded: {_isGrounded}");
    }

    private static float KmhToMs(float kmh) => kmh / 3.6f;
}