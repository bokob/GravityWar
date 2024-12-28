using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlanetDestory : MonoBehaviour
{
    public Color vertexColor = Color.red;   // ���� ����
    public Color edgeColor = Color.blue;     // ���� ����
    public float vertexSize = 0.2f;         // ���� ũ��

    MeshFilter meshFilter;


    public float craterRadius = 2f;   // ũ������ �ݰ�
    public float craterDepth = 0.5f; // ũ������ ����
    public Vector3 craterOffset = Vector3.zero; // ũ������ �߽� ������


    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ũ������ �߽� ��� (��: �浹 ����)
            Vector3 craterCenter = transform.position + craterOffset;

            // ũ������ ����
            CreateCrater(craterCenter, craterRadius, craterDepth);

            // �ݶ��̴� ������Ʈ
            UpdateCollider();
        }
    }

    void CreateCrater(Vector3 craterCenter, float radius, float depth)
    {
        if (meshFilter == null) return;

        Mesh mesh = meshFilter.mesh;

        // ���ؽ� ������ ��������
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            // ���� ��ǥ�� ��ȯ
            Vector3 worldVertex = transform.TransformPoint(vertices[i]);

            // ũ������ �߽ɰ� ���ؽ� �� �Ÿ� ���
            float distance = Vector3.Distance(worldVertex, craterCenter);

            if (distance <= radius)
            {
                // �Ÿ� ��ʷ� ���� ���
                float depthFactor = (1f - (distance / radius)) * depth;

                // ��ü �߽� �������� ��ǫ ���� ����
                Vector3 directionToCenter = (worldVertex - transform.position).normalized;
                vertices[i] -= directionToCenter * depthFactor;
            }
        }

        // ������ ���ؽ��� �޽��� ����
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // �޽� ���Ϳ� ������Ʈ�� �޽� ����
        meshFilter.mesh = mesh;
    }

    void UpdateCollider()
    {
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            // �ݶ��̴� ������ ���� sharedMesh�� ���Ҵ�
            meshCollider.sharedMesh = null; // ���� null�� ����
            meshCollider.sharedMesh = GetComponent<MeshFilter>().mesh; // ������ �޽� ���Ҵ�
        }
    }

    void OnDrawGizmos()
    {
        // ũ������ ���� ��ġ �����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + craterOffset, craterRadius);

        DrawVertexAndEdge();
    }

    void DrawVertexAndEdge()
    {
        if (meshFilter == null || meshFilter.sharedMesh == null) return;

        Mesh mesh = meshFilter.sharedMesh;

        // ���� ������
        Vector3[] vertices = mesh.vertices;

        // �ﰢ�� ������
        int[] triangles = mesh.triangles;

        // Debug.Log($"vertices: {vertices.Length}, triangles: {triangles.Length}");

        // Transform �������� ���� ��ǥ�� ��ȯ
        Transform meshTransform = transform;

        // ���� �׸���(���� ��ǥ ��������)
        Gizmos.color = vertexColor;
        foreach (Vector3 vertex in vertices)
        {
            Vector3 worldVertex = meshTransform.TransformPoint(vertex);
            Gizmos.DrawSphere(worldVertex, vertexSize);
        }

        // ���� �׸���
        Gizmos.color = edgeColor;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            // �ﰢ���� �� ���� �ε���
            int v1 = triangles[i];
            int v2 = triangles[i + 1];
            int v3 = triangles[i + 2];

            // ���� ��ǥ�� ��ȯ
            Vector3 worldV1 = meshTransform.TransformPoint(vertices[v1]);
            Vector3 worldV2 = meshTransform.TransformPoint(vertices[v2]);
            Vector3 worldV3 = meshTransform.TransformPoint(vertices[v3]);

            // �ﰢ���� ���� �׸���
            Gizmos.DrawLine(worldV1, worldV2);
            Gizmos.DrawLine(worldV2, worldV3);
            Gizmos.DrawLine(worldV3, worldV1);
        }
    }
}