using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlanetDestory : MonoBehaviour
{
    public Color vertexColor = Color.red;   // 정점 색상
    public Color edgeColor = Color.blue;     // 엣지 색상
    public float vertexSize = 0.2f;         // 정점 크기

    MeshFilter meshFilter;


    public float craterRadius = 2f;   // 크레이터 반경
    public float craterDepth = 0.5f; // 크레이터 깊이
    public Vector3 craterOffset = Vector3.zero; // 크레이터 중심 오프셋


    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 크레이터 중심 계산 (예: 충돌 지점)
            Vector3 craterCenter = transform.position + craterOffset;

            // 크레이터 생성
            CreateCrater(craterCenter, craterRadius, craterDepth);

            // 콜라이더 업데이트
            UpdateCollider();
        }
    }

    void CreateCrater(Vector3 craterCenter, float radius, float depth)
    {
        if (meshFilter == null) return;

        Mesh mesh = meshFilter.mesh;

        // 버텍스 데이터 가져오기
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            // 월드 좌표로 변환
            Vector3 worldVertex = transform.TransformPoint(vertices[i]);

            // 크레이터 중심과 버텍스 간 거리 계산
            float distance = Vector3.Distance(worldVertex, craterCenter);

            if (distance <= radius)
            {
                // 거리 비례로 깊이 계산
                float depthFactor = (1f - (distance / radius)) * depth;

                // 구체 중심 방향으로 움푹 들어가게 만듦
                Vector3 directionToCenter = (worldVertex - transform.position).normalized;
                vertices[i] -= directionToCenter * depthFactor;
            }
        }

        // 수정된 버텍스를 메쉬에 적용
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // 메쉬 필터에 업데이트된 메쉬 적용
        meshFilter.mesh = mesh;
    }

    void UpdateCollider()
    {
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            // 콜라이더 갱신을 위해 sharedMesh를 재할당
            meshCollider.sharedMesh = null; // 먼저 null로 설정
            meshCollider.sharedMesh = GetComponent<MeshFilter>().mesh; // 수정된 메쉬 재할당
        }
    }

    void OnDrawGizmos()
    {
        // 크레이터 생성 위치 디버깅
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + craterOffset, craterRadius);

        DrawVertexAndEdge();
    }

    void DrawVertexAndEdge()
    {
        if (meshFilter == null || meshFilter.sharedMesh == null) return;

        Mesh mesh = meshFilter.sharedMesh;

        // 정점 데이터
        Vector3[] vertices = mesh.vertices;

        // 삼각형 데이터
        int[] triangles = mesh.triangles;

        // Debug.Log($"vertices: {vertices.Length}, triangles: {triangles.Length}");

        // Transform 기준으로 월드 좌표로 변환
        Transform meshTransform = transform;

        // 정점 그리기(월드 좌표 기준으로)
        Gizmos.color = vertexColor;
        foreach (Vector3 vertex in vertices)
        {
            Vector3 worldVertex = meshTransform.TransformPoint(vertex);
            Gizmos.DrawSphere(worldVertex, vertexSize);
        }

        // 엣지 그리기
        Gizmos.color = edgeColor;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            // 삼각형의 세 정점 인덱스
            int v1 = triangles[i];
            int v2 = triangles[i + 1];
            int v3 = triangles[i + 2];

            // 월드 좌표로 변환
            Vector3 worldV1 = meshTransform.TransformPoint(vertices[v1]);
            Vector3 worldV2 = meshTransform.TransformPoint(vertices[v2]);
            Vector3 worldV3 = meshTransform.TransformPoint(vertices[v3]);

            // 삼각형의 엣지 그리기
            Gizmos.DrawLine(worldV1, worldV2);
            Gizmos.DrawLine(worldV2, worldV3);
            Gizmos.DrawLine(worldV3, worldV1);
        }
    }
}