using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class change : MonoBehaviour
{
    // [Inspector 설정 필요]
    public Sprite[] playerSprites;      // 상승 스프라이트 배열 (Index 0 = 초기, Index 1 = Stage1)
    public Sprite[] playerJumpSprites;  // 하강 스프라이트 배열
    
    // ⭐ 현재 스테이지 인덱스를 추적하는 변수 (0부터 시작)
    private int currentStageIndex = 0; 
    
    private player playerScript;

    public GameObject spawner;

    // ⭐ 점프 파워를 관리하는 배열 추가 (Inspector에서 설정해야 합니다)
    [Header("스테이지별 점프 파워 설정 (Index 0부터 채우세요)")]
    public float[] jumpPowers = { 15f, 18f, 20f }; // 예시 값
    public float[] superJumpPowers = { 30f, 35f, 40f }; // 예시 값

    void Start()
    {
        playerScript = FindObjectOfType<player>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 'Stage' 태그는 연속적인 스테이지 전환 트리거로 사용
        if (other.CompareTag("Stage"))
        {
            // 다음 스테이지 인덱스 계산
            int nextStageIndex = currentStageIndex + 1;

            // ⭐ 배열 길이 체크 및 인덱스 증가
            if (nextStageIndex >= playerSprites.Length || 
                nextStageIndex >= playerJumpSprites.Length ||
                nextStageIndex >= jumpPowers.Length ||
                nextStageIndex >= superJumpPowers.Length)
            {
                Debug.LogWarning($"더 이상 바꿀 스프라이트나 파워 데이터가 없습니다! Stage {nextStageIndex} 배열 부족.");
                return;
            }

            // 인덱스 업데이트
            currentStageIndex = nextStageIndex;

            // 한 번 작동 후 비활성화
            other.GetComponent<Collider2D>().enabled = false;

            // 스테이지 전환 코루틴 시작
            StartCoroutine(ChaChaCha(currentStageIndex));
        }

        if(other.CompareTag("ActiveOff"))
        {
            spawner.SetActive(false);
        }

        if(other.CompareTag("end"))
        {
            SceneManager.LoadScene("result");
        }
    }

    IEnumerator ChaChaCha(int stageIndex)
    {
        // 1. 시간 정지
        Time.timeScale = 0f;

        if (playerScript != null)
        {
            // 2. ⭐ 배열에서 현재 스테이지 인덱스에 맞는 파워와 스프라이트를 가져와 업데이트
            playerScript.ApplyStageUpgrade(
                jumpPowers[stageIndex],
                superJumpPowers[stageIndex],
                playerSprites[stageIndex],
                playerJumpSprites[stageIndex]
            );

            Debug.Log($"플레이어 스프라이트 및 파워 변경됨 (stage = {stageIndex})");
        }

        // 3. 1초 대기
        yield return new WaitForSecondsRealtime(1f);

        // 4. 시간 재개
        Time.timeScale = 1f;
    }
}