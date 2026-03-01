using UnityEngine;

public sealed class PlaneCrash : MonoBehaviour
{
    [Header("Impact")]
    [SerializeField] private float minImpactSpeed = 80f;

    [Header("Explosion")]
    [SerializeField] private GameObject explosionPrefab;

    [Header("Respawn")]
    [SerializeField] private bool respawn = true;
    [SerializeField] private float respawnDelay = 2.0f;
    [SerializeField] private Vector3 respawnPosition = new(0f, 300f, 0f);

    private Rigidbody _rb;
    private bool _crashed;
    private float _lastSpeed;
    private MonoBehaviour[] _disableOnCrash;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _disableOnCrash = GetComponents<MonoBehaviour>();
    }

    private void FixedUpdate()
    {
        if (_crashed) return;
        if (_rb != null)
            _lastSpeed = _rb.linearVelocity.magnitude;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_crashed) return;
        if (_lastSpeed < minImpactSpeed) return;

        Crash(collision.contacts[0].point);
    }

    private void Crash(Vector3 hitPoint)
    {
        _crashed = true;

        if (explosionPrefab != null)
            Instantiate(explosionPrefab, hitPoint, Quaternion.identity);

        foreach (var mb in _disableOnCrash)
        {
            if (mb == this) continue;
            mb.enabled = false;
        }

        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        // Esconde todos os filhos (mesh, helice, etc)
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);

        if (respawn)
            Invoke(nameof(Respawn), respawnDelay);
        else
            Destroy(gameObject, 0.1f);
    }

    private void Respawn()
    {
        _crashed = false;

        transform.position = respawnPosition;
        transform.rotation = Quaternion.identity;

        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        // Reativa todos os filhos
        foreach (Transform child in transform)
            child.gameObject.SetActive(true);

        foreach (var mb in _disableOnCrash)
            mb.enabled = true;
    }
}