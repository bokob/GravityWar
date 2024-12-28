using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject spherePrefab; // 구 프리팹
    public int playerCount = 4; // 플레이어 수
    public Vector3 areaCenter; // 구역의 중심
    public Vector3 areaSize; // 구역의 크기
    public float minDistance = 2f; // 구 간 최소 거리
    public Vector3 sizeRange = new Vector3(5f, 5f); // 구 크기 범위 (최소, 최대)

    private List<Vector3> spawnedPositions = new List<Vector3>(); // 생성된 위치 저장

    void Start()
    {
        SpawnSpheres();
    }

    void SpawnSpheres()
    {
        int spawnedCount = 0;

        while (spawnedCount < playerCount)
        {
            // 랜덤 위치 계산
            Vector3 randomPosition = new Vector3(
                Random.Range(areaCenter.x - areaSize.x / 2, areaCenter.x + areaSize.x / 2),
                Random.Range(areaCenter.y - areaSize.y / 2, areaCenter.y + areaSize.y / 2),
                Random.Range(areaCenter.z - areaSize.z / 2, areaCenter.z + areaSize.z / 2)
            );

            // 거리 체크
            bool isFarEnough = true;
            foreach (Vector3 pos in spawnedPositions)
            {
                if (Vector3.Distance(pos, randomPosition) < minDistance)
                {
                    isFarEnough = false;
                    break;
                }
            }

            // 조건 만족 시 구 생성
            if (isFarEnough)
            {
                GameObject sphere = Instantiate(spherePrefab, randomPosition, Quaternion.identity);

                // 랜덤 크기 설정
                float randomScale = Random.Range(sizeRange.x, sizeRange.y);
                sphere.transform.localScale = Vector3.one * randomScale;

                spawnedPositions.Add(randomPosition);
                spawnedCount++;
            }
        }
    }

    void OnDrawGizmos()
    {
        // 디버깅용으로 구역 시각화
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}
