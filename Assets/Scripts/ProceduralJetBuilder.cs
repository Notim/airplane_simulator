using UnityEngine;

public sealed class ProceduralJetBuilder : MonoBehaviour
{
    [SerializeField] private bool buildOnStart = true;
    [SerializeField] private float scale = 1.0f;

    [Header("Options")]
    [SerializeField] private bool addCanopy = true;
    [SerializeField] private bool addMissiles = true;

    [Header("Materials (optional)")]
    [SerializeField] private Material bodyMaterial;
    [SerializeField] private Material detailMaterial;

    private void Start()
    {
        if (buildOnStart)
            Build();
    }

    [ContextMenu("Build Jet")]
    public void Build()
    {
        ClearChildren();

        CreateBody();
        CreateNose();
        CreateWings();
        CreateTail();
        CreateEngine();

        if (addCanopy) CreateCanopy();
        if (addMissiles) CreateMissiles();
    }

    private void ClearChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);
#if UNITY_EDITOR
            if (!Application.isPlaying) DestroyImmediate(child.gameObject);
            else Destroy(child.gameObject);
#else
            Destroy(child.gameObject);
#endif
        }
    }

    private void CreateBody()
    {
        var body = CreateCube("Body", new Vector3(0f, 0f, 0f), new Vector3(0.7f, 0.35f, 2.8f));
        ApplyMaterial(body, bodyMaterial);
    }

    private void CreateNose()
    {
        var nose = CreateCube("Nose", new Vector3(0f, 0.02f, 1.55f), new Vector3(0.45f, 0.25f, 0.6f));
        ApplyMaterial(nose, bodyMaterial);
    }

    private void CreateWings()
    {
        var wingL = CreateCube("Wing_L", new Vector3(-0.85f, 0f, 0.1f), new Vector3(1.35f, 0.06f, 0.9f));
        wingL.transform.localRotation = Quaternion.Euler(0f, 12f, 0f);

        var wingR = CreateCube("Wing_R", new Vector3(0.85f, 0f, 0.1f), new Vector3(1.35f, 0.06f, 0.9f));
        wingR.transform.localRotation = Quaternion.Euler(0f, -12f, 0f);

        ApplyMaterial(wingL, bodyMaterial);
        ApplyMaterial(wingR, bodyMaterial);
    }

    private void CreateTail()
    {
        var tailWingL = CreateCube("TailWing_L", new Vector3(-0.55f, 0.05f, -1.25f), new Vector3(0.7f, 0.05f, 0.5f));
        tailWingL.transform.localRotation = Quaternion.Euler(0f, 10f, 0f);

        var tailWingR = CreateCube("TailWing_R", new Vector3(0.55f, 0.05f, -1.25f), new Vector3(0.7f, 0.05f, 0.5f));
        tailWingR.transform.localRotation = Quaternion.Euler(0f, -10f, 0f);

        var fin = CreateCube("Fin", new Vector3(0f, 0.45f, -1.32f), new Vector3(0.12f, 0.75f, 0.55f));

        ApplyMaterial(tailWingL, bodyMaterial);
        ApplyMaterial(tailWingR, bodyMaterial);
        ApplyMaterial(fin, bodyMaterial);
    }

    private void CreateEngine()
    {
        var engine = CreateCylinder("Engine", new Vector3(0f, -0.03f, -1.45f), new Vector3(0.45f, 0.45f, 0.45f));
        engine.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

        ApplyMaterial(engine, detailMaterial != null ? detailMaterial : bodyMaterial);
    }

    private void CreateCanopy()
    {
        var canopy = CreateCube("Canopy", new Vector3(0f, 0.22f, 0.55f), new Vector3(0.35f, 0.18f, 0.65f));
        ApplyMaterial(canopy, detailMaterial != null ? detailMaterial : bodyMaterial);
    }

    private void CreateMissiles()
    {
        var mL = CreateCylinder("Missile_L", new Vector3(-1.1f, -0.12f, 0.0f), new Vector3(0.12f, 0.35f, 0.12f));
        mL.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

        var mR = CreateCylinder("Missile_R", new Vector3(1.1f, -0.12f, 0.0f), new Vector3(0.12f, 0.35f, 0.12f));
        mR.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);

        ApplyMaterial(mL, detailMaterial != null ? detailMaterial : bodyMaterial);
        ApplyMaterial(mR, detailMaterial != null ? detailMaterial : bodyMaterial);
    }

    private GameObject CreateCube(string name, Vector3 localPos, Vector3 localScale)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(transform, false);
        go.transform.localPosition = localPos * scale;
        go.transform.localScale = localScale * scale;
        return go;
    }

    private GameObject CreateCylinder(string name, Vector3 localPos, Vector3 localScale)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        go.name = name;
        go.transform.SetParent(transform, false);
        go.transform.localPosition = localPos * scale;
        go.transform.localScale = localScale * scale;
        return go;
    }

    private void ApplyMaterial(GameObject go, Material mat)
    {
        if (mat == null) return;
        var r = go.GetComponent<Renderer>();
        if (r != null) r.sharedMaterial = mat;
    }
}