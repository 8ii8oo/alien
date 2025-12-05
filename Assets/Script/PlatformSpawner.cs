using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("플랫폼 프리팹 랜덤 풀")]
    public GameObject[] platformPrefabs;

    [Header("플레이어")]
    public Transform player;

    [Header("스폰 설정")]
    public float yGap = 10f;
    public float spawnAheadDistance = 20f;
    public float deleteDistance = 15f;

    [Header("X 설정")]
    public bool useRandomX = false;
    public float fixedX = 0f;
    public float randomXMin = -5f;
    public float randomXMax = 5f;

    private float lastSpawnY = 0f;
    private List<GameObject> platforms = new List<GameObject>();

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player 참조 없음!");
            return;
        }

        // 🔥 처음 시작할 때 위로 5개 미리 생성해줘야 게임이 끊기지 않음
        for (int i = 0; i < 5; i++)
        {
            SpawnPlatform(i * yGap);
        }

        lastSpawnY = (5 - 1) * yGap;
    }

    void Update()
    {
        if (player == null) return;

        // 🔥 플레이어가 lastSpawnY -20 내에 오면 새로운 발판 생성
        if (player.position.y + spawnAheadDistance > lastSpawnY)
        {
            lastSpawnY += yGap;
            SpawnPlatform(lastSpawnY);
        }

        CleanupPlatforms();
    }

    void SpawnPlatform(float y)
    {
        GameObject prefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];

        float x = useRandomX ? Random.Range(randomXMin, randomXMax) : fixedX;

        Vector3 pos = new Vector3(x, y, 0);
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);
        platforms.Add(obj);
    }

    void CleanupPlatforms()
    {
        float py = player.position.y;

        for (int i = platforms.Count - 1; i >= 0; i--)
        {
            if (py - platforms[i].transform.position.y > deleteDistance)
            {
                Destroy(platforms[i]);
                platforms.RemoveAt(i);
            }
        }
    }
}
