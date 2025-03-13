using UnityEngine;

public class ExplosionCreator : MonoBehaviour
{
    public Mesh mesh;
    public GameObject ParticlePrefab;
    public Texture2D tex;
    public float ExplosionForce = 1;
    public float RandomVariationAmount = 0.5f;
    public float TorqueAmount = 1;
    public Vector3 Scale;
    public bool go;
    public OneShotSound ExplosionSound;

    void Update()
    {
        if (go)
        {
            Create();
            go = false;
        }
    }

    public void Create()
    {
        if (ExplosionSound)
        {
            Instantiate(ExplosionSound);
        }
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            var v1 = mesh.vertices[mesh.triangles[i]];
            var v2 = mesh.vertices[mesh.triangles[i + 1]];
            var v3 = mesh.vertices[mesh.triangles[i + 2]];

            var u1 = mesh.uv[mesh.triangles[i]];
            var u2 = mesh.uv[mesh.triangles[i + 1]];
            var u3 = mesh.uv[mesh.triangles[i + 2]];

            CreateTriangle(new[] { v1, v2, v3 }, new[] { u1, u2, u3 }, tex);
        }
    }

    void CreateTriangle(Vector3[] vertices, Vector2[] uvs, Texture2D tex)
    {
        vertices[0] *= 0.5f * Scale.x;
        vertices[1] *= 0.5f * Scale.y;
        vertices[2] *= 0.5f * Scale.z;

        var particle = Instantiate(ParticlePrefab);
        var filter = particle.GetComponent<MeshFilter>();
        var renderer = particle.GetComponent<MeshRenderer>();
        var rb = particle.GetComponent<Rigidbody2D>();

        renderer.material.mainTexture = tex;

        var midpoint = (vertices[0] + vertices[1] + vertices[2]) * 0.333f;
        vertices[0] -= midpoint;
        vertices[1] -= midpoint;
        vertices[2] -= midpoint;

        (midpoint.y, midpoint.z) = (midpoint.z, midpoint.y);

        particle.transform.position = transform.position + midpoint;
        particle.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);

        filter.mesh = new Mesh();
        filter.mesh.SetVertices(vertices);
        filter.mesh.SetIndices(new[] { 0, 1, 2 }, MeshTopology.Triangles, 0);
        filter.mesh.SetUVs(0, uvs);
        filter.mesh.RecalculateNormals();
        filter.mesh.RecalculateBounds();

        var rand = Random.onUnitSphere;
        float multiplier = 1f / midpoint.magnitude;
        rb.AddForce(midpoint * ExplosionForce * multiplier + rand * RandomVariationAmount, ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-1f, 1f) * TorqueAmount, ForceMode2D.Impulse);
    }
}