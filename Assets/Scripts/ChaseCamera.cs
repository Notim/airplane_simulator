using UnityEngine;

public sealed class ChaseCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new(0f, 3f, -10f);

    [Header("Position")]
    [SerializeField] private float posFollowSpeed = 10f;    // quao rapido segue a posicao

    [Header("Rotation")]
    [SerializeField] private float yawFollowSpeed  = 8f;    // roll/yaw seguem rapido
    [SerializeField] private float pitchFollowSpeed = 3f;   // pitch tem atraso

    private Vector3 _currentPos;
    private float _currentYaw;
    private float _currentPitch;
    private float _currentRoll;

    private void Start()
    {
        if (target == null) return;

        _currentPos   = target.TransformPoint(offset);
        _currentYaw   = target.eulerAngles.y;
        _currentPitch = target.eulerAngles.x;
        _currentRoll  = target.eulerAngles.z;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        float dt = Time.deltaTime;

        // ── Posição segue rápido (paralax leve) ───────────────
        Vector3 desiredPos = target.TransformPoint(offset);
        _currentPos = Vector3.Lerp(_currentPos, desiredPos, posFollowSpeed * dt);
        transform.position = _currentPos;

        // ── Rotação: separa os 3 eixos ────────────────────────
        // Yaw e Roll seguem normal
        _currentYaw  = Mathf.LerpAngle(_currentYaw,  target.eulerAngles.y, yawFollowSpeed  * dt);
        _currentRoll = Mathf.LerpAngle(_currentRoll, target.eulerAngles.z, yawFollowSpeed  * dt);

        // Pitch tem atraso — efeito de inércia ao subir/descer
        _currentPitch = Mathf.LerpAngle(_currentPitch, target.eulerAngles.x, pitchFollowSpeed * dt);

        transform.rotation = Quaternion.Euler(_currentPitch, _currentYaw, _currentRoll);
    }
}