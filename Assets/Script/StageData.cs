using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/StageData")]
public class StageData : ScriptableObject
{
    [Header("Background")]
    public GameObject backgroundPrefab; // 전체 배경 프리팹 (씬 원점에 배치)

    [Header("Tile / Object Prefabs")]
    public GameObject[] tilePrefabs;    // platform / booster 등 (TileGenerator에 전달)
    public GameObject[] objectPrefabs;  // 추가 오브젝트(아이템, 장애물 등)

    [Header("Player Upgrade")]
    public float playerJumpPower = 5f;
    public float playerSuperJumpPower = 20f;
    public Sprite playerSprite;

    [Header("Stage Timings")]
    public float jumpPhaseDuration = 40f;       // 플레이어 자동 점프 구간 길이
    public float transitionDuration = 10f;      // 배경 스크롤 시간
    public float postTransitionHold = 10f;      // 전환 후 고정(이미지 유지) 시간

    [Header("Tile Settings")]
    public float spawnDistanceAhead = 20f;      // 플레이어 위로 얼마나 채울지
}
