using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject spherePrefab; // �� ������
    public int playerCount = 4; // �÷��̾� ��
    public Vector3 areaCenter; // ������ �߽�
    public Vector3 areaSize; // ������ ũ��
    public float minDistance = 2f; // �� �� �ּ� �Ÿ�
    public Vector3 sizeRange = new Vector3(5f, 5f); // �� ũ�� ���� (�ּ�, �ִ�)

    private List<Vector3> spawnedPositions = new List<Vector3>(); // ������ ��ġ ����

    void Start()
    {
        SpawnSpheres();
    }

    void SpawnSpheres()
    {
        int spawnedCount = 0;

        while (spawnedCount < playerCount)
        {
            // ���� ��ġ ���
            Vector3 randomPosition = new Vector3(
                Random.Range(areaCenter.x - areaSize.x / 2, areaCenter.x + areaSize.x / 2),
                Random.Range(areaCenter.y - areaSize.y / 2, areaCenter.y + areaSize.y / 2),
                Random.Range(areaCenter.z - areaSize.z / 2, areaCenter.z + areaSize.z / 2)
            );

            // �Ÿ� üũ
            bool isFarEnough = true;
            foreach (Vector3 pos in spawnedPositions)
            {
                if (Vector3.Distance(pos, randomPosition) < minDistance)
                {
                    isFarEnough = false;
                    break;
                }
            }

            // ���� ���� �� �� ����
            if (isFarEnough)
            {
                GameObject sphere = Instantiate(spherePrefab, randomPosition, Quaternion.identity);

                // ���� ũ�� ����
                float randomScale = Random.Range(sizeRange.x, sizeRange.y);
                sphere.transform.localScale = Vector3.one * randomScale;

                spawnedPositions.Add(randomPosition);
                spawnedCount++;
            }
        }
    }

    void OnDrawGizmos()
    {
        // ���������� ���� �ð�ȭ
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}
