using UnityEngine;

public sealed class PropellerSpin : MonoBehaviour
{
    [SerializeField] private float minRPM = 800f;
    [SerializeField] private float maxRPM = 3000f;
    [SerializeField] private PlaneController plane;

    private void Update()
    {
        float throttle = plane != null
            ? Mathf.Clamp01(plane.TargetSpeedMs / plane.MaxSpeedMs)
            : 1f;

        float rpm = Mathf.Lerp(minRPM, maxRPM, throttle);
        float degreesPerSecond = rpm * 6f;

        transform.Rotate(Vector3.forward, degreesPerSecond * Time.deltaTime, Space.Self);
    }
}