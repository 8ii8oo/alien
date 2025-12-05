using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour
{
    public StageData[] stages;
    public Transform stageRoot; // 배경/타일들을 붙일 부모(빈 오브젝트)
    public CameraMove cameraMove;
    public PlayerJumpManager playerJump;
    public TileGenerator tileGenerator;
    public player player;

    private float screenHeight;
    private int currentStageIndex = 0;
    private GameObject currentBG;
    private GameObject nextBG;

    void Start()
    {
        if (stages == null || stages.Length == 0)
        {
            Debug.LogError("StageManager: stages not assigned!");
            return;
        }

        if (cameraMove == null) cameraMove = FindObjectOfType<CameraMove>();
        if (playerJump == null) playerJump = FindObjectOfType<PlayerJumpManager>();
        if (tileGenerator == null) tileGenerator = FindObjectOfType<TileGenerator>();
        if (player == null) player = FindObjectOfType<player>();

        screenHeight = Camera.main.orthographicSize * 2f;

        StartCoroutine(RunStages());
    }

    IEnumerator RunStages()
    {
        // 처음 스테이지 세팅
        yield return StartStage(currentStageIndex);

        // 루프: 현재는 마지막 스테이지가 끝나면 멈춤
        while (currentStageIndex < stages.Length - 1)
        {
            StageData stage = stages[currentStageIndex];

            // 1) 플레이어 자동 점프 구간
            playerJump.StartJumpPhase();
            tileGenerator.StartGenerate();
            tileGenerator.ConfigureForStage(stage.tilePrefabs, stage.objectPrefabs, stage.spawnDistanceAhead, player.transform);
            player.ApplyStageUpgrade(stage.playerJumpPower, stage.playerSuperJumpPower, stage.playerSprite);

            yield return new WaitForSeconds(stage.jumpPhaseDuration);

            // 2) 다음 배경 준비
            int nextIndex = currentStageIndex + 1;
            StageData nextStage = stages[nextIndex];

            // instantiate next BG at top
            nextBG = Instantiate(nextStage.backgroundPrefab, Vector3.up * screenHeight, Quaternion.identity, stageRoot);

            // 3) 멈춤 및 전환
            playerJump.StopJumpPhase();
            tileGenerator.StopGenerate();
            cameraMove.SetFixed();

            // move current & next down over transitionDuration
            yield return StartCoroutine(ScrollTransition(currentBG.transform, nextBG.transform, stage.transitionDuration));

            // destroy old background
            Destroy(currentBG);
            currentBG = nextBG;
            nextBG = null;
            currentStageIndex = nextIndex;

            // 4) 보류(post hold)
            yield return new WaitForSeconds(nextStage.postTransitionHold);

            // 5) 재개: 세팅 갱신
            cameraMove.ReleaseFixed();
            // apply next stage player upgrade & generator config (so new tiles/objects use new prefabs)
            player.ApplyStageUpgrade(nextStage.playerJumpPower, nextStage.playerSuperJumpPower, nextStage.playerSprite);
            tileGenerator.ConfigureForStage(nextStage.tilePrefabs, nextStage.objectPrefabs, nextStage.spawnDistanceAhead, player.transform);
            playerJump.StartJumpPhase();
            tileGenerator.StartGenerate();

            // 계속 루프 (다음 stage.loop)
        }

        // 마지막 스테이지 도달 후: 그냥 계속 그 스테이지 루프 유지
        Debug.Log("StageManager: reached final stage.");
    }

    IEnumerator StartStage(int idx)
    {
        StageData sd = stages[idx];

        // 배경 생성 (원점)
        currentBG = Instantiate(sd.backgroundPrefab, Vector3.zero, Quaternion.identity, stageRoot);

        // 카메라 해제(따라가도 됨)
        cameraMove.ReleaseFixed();

        // 초기 플레이어 세팅
        player.ApplyStageUpgrade(sd.playerJumpPower, sd.playerSuperJumpPower, sd.playerSprite);

        // 타일 세팅 (단, 실제 생성은 JumpPhase 시작 시)
        tileGenerator.ConfigureForStage(sd.tilePrefabs, sd.objectPrefabs, sd.spawnDistanceAhead, player.transform);

        yield return null;
    }

    IEnumerator ScrollTransition(Transform cur, Transform next, float duration)
    {
        float start = Time.time;
        Vector3 aStart = cur.position;
        Vector3 bStart = next.position;

        Vector3 aEnd = aStart + Vector3.down * screenHeight;
        Vector3 bEnd = bStart + Vector3.down * screenHeight;

        while (Time.time < start + duration)
        {
            float t = (Time.time - start) / duration;
            cur.position = Vector3.Lerp(aStart, aEnd, t);
            next.position = Vector3.Lerp(bStart, bEnd, t);
            yield return null;
        }

        cur.position = aEnd;
        next.position = bEnd;
    }
}
