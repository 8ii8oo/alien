using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [Header("Spawn range")]
    public float spawnXMin = -6f;
    public float spawnXMax = 6f;

    [Header("Gap")]
    public float tileGapMin = 2f;
    public float tileGapMax = 4f;

    [Header("Generation control")]
    public bool isGenerating = false;
    public float generateAheadDistance = 20f; // 플레이어 위로 얼마만큼 채울지

    public Transform player;

    private float highestY;
    private GameObject[] tilePrefabs;   // injected per stage
    private GameObject[] objectPrefabs; // injected per stage

    void Start()
    {
        if (player != null) highestY = player.position.y;
    }

    void Update()
    {
        if (!isGenerating || player == null) return;

        while (highestY < player.position.y + generateAheadDistance)
            SpawnTile();
    }

    public void ConfigureForStage(GameObject[] tiles, GameObject[] objects, float aheadDistance, Transform playerTransform)
    {
        tilePrefabs = tiles;
        objectPrefabs = objects;
        generateAheadDistance = aheadDistance;
        player = playerTransform;

        if (player != null && highestY < player.position.y) highestY = player.position.y;
    }

    void SpawnTile()
    {
        float gap = Random.Range(tileGapMin, tileGapMax);
        highestY += gap;

        float randX = Random.Range(spawnXMin, spawnXMax);

        // pick random tile prefab from provided list
        GameObject chosenTile = null;
        if (tilePrefabs != null && tilePrefabs.Length > 0)
            chosenTile = tilePrefabs[Random.Range(0, tilePrefabs.Length)];

        if (chosenTile == null) return;

        GameObject tile = Instantiate(chosenTile, new Vector3(randX, highestY, 0), Quaternion.identity);
        // tag should be set on prefab ideally; fallback:
        if (tile.CompareTag("Untagged"))
        {
            // try auto-tag by name
            if (tile.name.ToLower().Contains("boost")) tile.tag = "booster";
            else tile.tag = "platform";
        }

        // occasionally spawn an object near this Y
        if (objectPrefabs != null && objectPrefabs.Length > 0 && Random.value < 0.15f) // 15% 확률
        {
            GameObject obj = objectPrefabs[Random.Range(0, objectPrefabs.Length)];
            Instantiate(obj, new Vector3(Random.Range(spawnXMin, spawnXMax), highestY + Random.Range(0.4f, 1.6f), 0), Quaternion.identity);
        }
    }

    public void StartGenerate() => isGenerating = true;
    public void StopGenerate() => isGenerating = false;
}
